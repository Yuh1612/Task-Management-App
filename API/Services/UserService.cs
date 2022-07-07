using API.DTOs.Projects;
using API.DTOs.Users;
using API.Exceptions;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Interfaces;
using Domain.Interfaces.Authentications;
using MediatR;

namespace API.Services
{
    public class UserService : BaseService
    {
        public readonly ISender _mediator;
        public readonly IJwtHandler _jwtHandler;
        public readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, ISender mediator, IJwtHandler jwtHandler, IMapper mapper) : base(unitOfWork)
        {
            _mediator = mediator;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
        }

        public async Task<AddUserResponse> AddUserAsync(AddUserRequest request)
        {
            try
            {
                await UnitOfWork.BeginTransaction();
                var user = new User(request.UserName,
                    BCrypt.Net.BCrypt.HashPassword(request.Password),
                    request.Name,
                    request.Email,
                    request.Age,
                    request.BirthDay);
                await UnitOfWork.userRepository.InsertAsync(user);

                await UnitOfWork.CommitTransaction();
                var response = _mapper.Map<AddUserResponse>(user);

                return response;
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<LoginResponse> ValidateUser(LoginRequest request)
        {
            try
            {
                var user = await UnitOfWork.userRepository.FindOneByUserName(request.UserName);
                if (user == null) throw new UserNotFoundException("User not found");
                if (user.CheckPassword(request.Password) == false) throw new InvalidUserPasswordException("Wrong password");
                var loginresponse = new LoginResponse
                {
                    AccessToken = _jwtHandler.GenerateAccessToken(user),
                    RefreshToken = _jwtHandler.GenerateRefreshToken()
                };
                await UnitOfWork.BeginTransaction();
                user.UpdateRefreshToken(loginresponse.RefreshToken);
                UnitOfWork.userRepository.Update(user);
                await UnitOfWork.CommitTransaction();
                return loginresponse;
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                int userId = _jwtHandler.ValidateAccessToken(request.AccessToken);
                var user = await UnitOfWork.userRepository.FindAsync(userId);
                if (user == null) throw new Exception("Invalid Token");
                if (user.CheckRefreshToken(request.RefreshToken) == false) throw new Exception("Invalid Token");
                var refreshTokenResponse = new RefreshTokenResponse
                {
                    AccessToken = _jwtHandler.GenerateAccessToken(user),
                    RefreshToken = _jwtHandler.GenerateRefreshToken()
                };
                await UnitOfWork.BeginTransaction();
                user.UpdateRefreshToken(refreshTokenResponse.RefreshToken);
                UnitOfWork.userRepository.Update(user);
                await UnitOfWork.CommitTransaction();
                return refreshTokenResponse;
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, int? userId)
        {
            try
            {
                if (userId == null) throw new Exception("Invalid user");
                await UnitOfWork.BeginTransaction();
                var user = await UnitOfWork.userRepository.FindAsync(userId);
                if (user == null) throw new UserNotFoundException("User Not Found");
                var password = request.Password == null ? request.Password : BCrypt.Net.BCrypt.HashPassword(request.Password);
                user.Update(userId, password, request.Name, request.Email, request.Age, request.BirthDay);
                UnitOfWork.userRepository.Update(user);
                await UnitOfWork.CommitTransaction();
                var updateUserResponse = _mapper.Map<UpdateUserResponse>(user);
                return updateUserResponse;
            }
            catch
            {
                await UnitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<GetOneResponse> GetOne(GetOneRequest request)
        {
            var user = await UnitOfWork.userRepository.FindAsync(request.Id);
            if (user == null) throw new UserNotFoundException("User Not Found");
            var projects = await UnitOfWork.projectRepository.GetAllByUser(user);
            var response = _mapper.Map<GetOneResponse>(user);
            response.Projects = _mapper.Map<List<ProjectDTO>>(projects);
            return response;
        }
    }
}