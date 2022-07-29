using API.DTOs.Users;
using API.Extensions;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Interfaces;
using Domain.Interfaces.Authentications;
using System.Net;

namespace API.Services
{
    public class UserService : BaseService
    {
        public readonly IJwtHandler _jwtHandler;

        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IMapper mapper, IJwtHandler jwtHandler)
            : base(unitOfWork, contextAccessor, mapper)
        {
            _jwtHandler = jwtHandler;
        }

        public async Task<UserDetailDTO> CreateUser(CreateUserDTO request)
        {
            if (await _unitOfWork.userRepository.IsExistUserName(request.UserName))
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Username is already existed!");
            try
            {
                var user = _mapper.Map<User>(request);
                user.HashPassWord();
                await _unitOfWork.BeginTransaction();
                await _unitOfWork.userRepository.InsertAsync(user);
                user.AddCreateUserDomainEvent();
                await _unitOfWork.CommitTransaction(false);
                return _mapper.Map<UserDetailDTO>(user);
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async Task UpdateUser(UpdateUserDTO request)
        {
            var user = await _unitOfWork.userRepository.FindAsync(GetCurrentUserId());
            try
            {
                await _unitOfWork.BeginTransaction();
                user.Update(request.Password, request.Name, request.Email, request.Age, request.BirthDay);
                _unitOfWork.userRepository.Update(user);
                await _unitOfWork.CommitTransaction();
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }

        public async Task<UserMinDTO> GetOne(Guid Id)
        {
            var user = await _unitOfWork.userRepository.FindAsync(Id);
            if (user == null) throw new HttpResponseException(HttpStatusCode.NotFound, "User is not found!");

            var response = _mapper.Map<UserMinDTO>(user);
            _mapper.Map(user.ProjectMembers.Select(s => s.Project), response.Projects);

            return response;
        }

        public async Task<UserDetailDTO> GetUserInfo(string AccessToken)
        {
            var userId = _jwtHandler.GetUserId(AccessToken);

            var user = await _unitOfWork.userRepository.FindAsync(userId);
            if (user == null) throw new HttpResponseException(HttpStatusCode.NotFound, "User is not found!");

            var response = _mapper.Map<UserDetailDTO>(user);

            return response;
        }

        public async Task<UserTokenDTO> ValidateUser(UserAccountDTO request)
        {
            var user = await _unitOfWork.userRepository.GetOneByUserName(request.UserName);
            if (user == null || !user.HasPassword(request.Password))
                throw new HttpResponseException(HttpStatusCode.Unauthorized, "User is not found!");

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
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
            return token;
        }

        public async Task<UserTokenDTO> RefreshToken(UserTokenDTO request)
        {
            var userId = _jwtHandler.ValidateAccessToken(request.AccessToken);

            var user = await _unitOfWork.userRepository.FindAsync(userId);
            if (user == null || !user.HasRefreshToken(request.RefreshToken) || user.IsRefreshTokenExpired())
                throw new HttpResponseException(HttpStatusCode.Unauthorized, "Something went wrong!");

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
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
            return token;
        }
    }
}