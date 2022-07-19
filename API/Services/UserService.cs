using API.DTOs.Users;
using API.Extensions;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Interfaces;
using Domain.Interfaces.Authentications;
using Domain.Interfaces.DomainServices;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace API.Services
{
    public class UserService : BaseService
    {
        public readonly IJwtHandler _jwtHandler;
        private readonly IUserManager _userManager;

        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper,
            IAuthorizationService authorizationService, IJwtHandler jwtHandler, IUserManager userManager)
            : base(unitOfWork, contextAccessor, mapper, authorizationService)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        public async Task<UserMinDTO> GetOne(Guid Id)
        {
            var user = await _unitOfWork.userRepository.FindAsync(Id);
            if (user == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            var response = _mapper.Map<UserMinDTO>(user);
            return response;
        }

        public async Task<UserDTO> GetInfo(string AccessToken)
        {
            var userId = _jwtHandler.GetUserId(AccessToken);
            var user = await _unitOfWork.userRepository.FindAsync(userId);
            if (user == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            var projects = await _unitOfWork.projectRepository.GetAllByUser(user);
            var response = _mapper.Map<UserDTO>(user);
            _mapper.Map(projects, response.Projects);
            return response;
        }

        public async Task<UserDetailDTO> CreateUser(CreateUserDTO request)
        {
            if (await _unitOfWork.userRepository.IsExistUserName(request.UserName)) throw new HttpResponseException(HttpStatusCode.BadRequest);
            var user = await _userManager.AddUser(_mapper.Map<User>(request));
            if (user == null) throw new HttpResponseException(HttpStatusCode.BadRequest);
            return _mapper.Map<UserDetailDTO>(user);
        }

        public async Task<UserTokenDTO> ValidateUser(UserAccountDTO request)
        {
            var user = await _unitOfWork.userRepository.GetOneByUserName(request.UserName);
            if (user == null || !user.HasPassword(request.Password)) throw new HttpResponseException(HttpStatusCode.Unauthorized);
            var token = new UserTokenDTO(_jwtHandler.GenerateAccessToken(user), _jwtHandler.GenerateRefreshToken());
            try
            {
                await _unitOfWork.BeginTransaction();
                user.UpdateRefreshToken(token.RefreshToken);
                _unitOfWork.userRepository.Update(user);
                await _unitOfWork.CommitTransaction(false);
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return token;
        }

        public async Task<UserTokenDTO> RefreshToken(UserTokenDTO request)
        {
            var userId = _jwtHandler.ValidateAccessToken(request.AccessToken);
            var user = await _unitOfWork.userRepository.FindAsync(userId);
            if (user == null || !user.HasRefreshToken(request.RefreshToken) || user.IsRefreshTokenExpired()) throw new HttpResponseException(HttpStatusCode.Unauthorized);
            var token = new UserTokenDTO(_jwtHandler.GenerateAccessToken(user), _jwtHandler.GenerateRefreshToken());
            try
            {
                await _unitOfWork.BeginTransaction();
                user.UpdateRefreshToken(token.RefreshToken);
                _unitOfWork.userRepository.Update(user);
                await _unitOfWork.CommitTransaction(false);
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return token;
        }

        public async Task UpdateUser(UpdateUserDTO request)
        {
            var oldUser = await GetCurrentUser();
            var newUser = _mapper.Map<User>(request);
            if (!await _userManager.UpdateUser(oldUser, newUser)) throw new HttpResponseException(HttpStatusCode.BadRequest);
        }
    }
}