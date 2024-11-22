using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using SolarPanelCalculatorApi.Application.DTO;
using SolarPanelCalculatorApi.Domain.Models;
using SolarPanelCalculatorApi.Domain.Interfaces;

namespace SolarPanelCalculatorApi.Presentation.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de usuários e autenticação.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Inicializa uma nova instância de UsersController.
        /// </summary>
        /// <param name="userService">Serviço de gerenciamento de usuários.</param>
        /// <param name="mapper">Mapeador de objetos.</param>
        /// <param name="configuration">Configuração da aplicação.</param>
        public UsersController(IUserService userService, IMapper mapper, IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Autentica um usuário e gera um token JWT.
        /// </summary>
        /// <param name="model">Dados de autenticação (email e senha).</param>
        /// <returns>Usuário autenticado e token JWT.</returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateDto model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            var user = await _userService.Authenticate(model.Email, model.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var token = GenerateJwtToken(user);
            var userDto = _mapper.Map<UserDto>(user);

            return Ok(new
            {
                user = userDto,
                token
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAll();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(long id)
        {
            var user = await _userService.GetById(id);

            if (user == null)
                return NotFound(new { message = "User not found" });

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        /// <summary>
        /// Registra um novo usuário no sistema.
        /// </summary>
        /// <param name="model">Dados do usuário a ser registrado.</param>
        /// <returns>Status de sucesso ou erro.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            Console.WriteLine($"Received registration request: {model.Email}");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(model);

            try
            {
                await _userService.Register(user, model.Password);
                Console.WriteLine($"User registered successfully: {model.Email}");
                return Ok();
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine($"Error registering user: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Gera um token JWT para o usuário autenticado.
        /// </summary>
        /// <param name="user">Dados do usuário autenticado.</param>
        /// <returns>Token JWT.</returns>
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, "User")
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Configuração de expiração
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
