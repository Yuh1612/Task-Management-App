using API.DTOs.Projects;
using API.DTOs.Users;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Interfaces;
using Domain.Interfaces.Authentications;

namespace API.Services
{
    public class UserService : BaseService
    {
        public readonly IJwtHandler _jwtHandler;
        public readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IJwtHandler jwtHandler, IMapper mapper) : base(unitOfWork)
        {
            _jwtHandler = jwtHandler;
            _mapper = mapper;
        }

        public async Task<AddUserResponse> AddUserAsync(AddUserRequest request)
        {
            try
            {
                await UnitOfWork.BeginTransaction();
                var user = new User(request.UserName,
                    request.Password,
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
                var user = await UnitOfWork.userRepository.GetOneByUserName(request.UserName);
                if (user == null) throw new KeyNotFoundException(nameof(user));

                if (user.HasPassword(request.Password) == false) throw new InvalidOperationException(nameof(request.Password));
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
                if (user == null) throw new KeyNotFoundException(nameof(user));
                if (user.HasRefreshToken(request.RefreshToken) == false) throw new ArgumentException(nameof(request.RefreshToken));
                if (user.IsRefreshTokenExpired() == true) throw new ArgumentException(nameof(request.RefreshToken));
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
                if (userId == null) throw new KeyNotFoundException(nameof(userId));

                await UnitOfWork.BeginTransaction();
                var user = await UnitOfWork.userRepository.FindAsync(userId);

                if (user == null) throw new DirectoryNotFoundException(nameof(user));

                user.Update(request.Password, request.Name, request.Email, request.Age, request.BirthDay);
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

        public async Task<GetOneUserResponse> GetOne(GetOneUserRequest request)
        {
            var user = await UnitOfWork.userRepository.FindAsync(request.Id);
            if (user == null) throw new KeyNotFoundException(nameof(user));
            var projects = await UnitOfWork.projectRepository.GetAllByUser(user);
            var response = _mapper.Map<GetOneUserResponse>(user);
            response.Projects = _mapper.Map<List<ProjectDTO>>(projects);
            return response;
        }
    }
}