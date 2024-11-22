using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using SolarPanelCalculatorApi.Application.DTO;
using SolarPanelCalculatorApi.Domain.Models;
using SolarPanelCalculatorApi.Domain.Interfaces;

namespace SolarPanelCalculatorApi.Presentation.Controllers
{
    /// <summary>
    /// Controller para gerenciar dispositivos/aparelhos do usuário.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AppliancesController : ControllerBase
    {
        private readonly IApplianceService _applianceService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        /// <summary>
        /// Inicializa uma nova instância de AppliancesController.
        /// </summary>
        /// <param name="applianceService">Serviço para gerenciamento de dispositivos.</param>
        /// <param name="mapper">Mapeador de objetos.</param>
        /// <param name="userService">Serviço para gerenciamento de usuários.</param>
        public AppliancesController(IApplianceService applianceService, IMapper mapper, IUserService userService)
        {
            _applianceService = applianceService;
            _mapper = mapper;
            _userService = userService;
        }

        /// <summary>
        /// Obtém todos os dispositivos/aparelhos do usuário autenticado.
        /// </summary>
        /// <returns>Lista de dispositivos/aparelhos.</returns>
        [HttpGet]
        public async Task<IActionResult> GetUserAppliances()
        {
            var userId = long.Parse(User.FindFirst("id")?.Value);
            var appliances = await _applianceService.GetAppliancesByUserId(userId);
            var appliancesDto = _mapper.Map<IEnumerable<ApplianceDto>>(appliances);
            foreach (var appliance in appliancesDto)
            {
                Console.WriteLine($"Appliance ID: {appliance.Id}, Name: {appliance.ApplianceName}, Power: {appliance.PowerConsumption}");
            }
            return Ok(appliancesDto);
        }

        /// <summary>
        /// Obtém dispositivos/aparelhos do usuário autenticado de forma paginada.
        /// </summary>
        /// <param name="page">Número da página (começa em 0).</param>
        /// <param name="pageSize">Número de itens por página.</param>
        /// <returns>Dispositivos paginados com informações de paginação.</returns>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedUserAppliances(
            [FromQuery] int page = 0, [FromQuery] int pageSize = 5)
        {
            var userId = long.Parse(User.FindFirst("id")?.Value);
            var appliances = await _applianceService.GetPagedAppliancesByUserId(userId, page, pageSize);

            var appliancesDto = _mapper.Map<IEnumerable<ApplianceDto>>(appliances);
            return Ok(new { currentPage = page, pageSize, items = appliancesDto });
        }

        /// <summary>
        /// Adiciona um novo dispositivo/aparelho para o usuário autenticado.
        /// </summary>
        /// <param name="applianceDto">Dados do dispositivo a ser adicionado.</param>
        /// <returns>Status de sucesso ou falha.</returns>
        [HttpPost]
        public async Task<IActionResult> AddAppliance([FromBody] ApplianceDto applianceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appliance = _mapper.Map<Appliance>(applianceDto);
            appliance.UserId = long.Parse(User.FindFirst("id")?.Value);
            await _applianceService.AddAppliance(appliance);

            return Ok();
        }

        /// <summary>
        /// Atualiza os dados de um dispositivo/aparelho existente.
        /// </summary>
        /// <param name="id">ID do dispositivo a ser atualizado.</param>
        /// <param name="applianceDto">Dados atualizados do dispositivo.</param>
        /// <returns>Status de sucesso ou falha.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppliance(long id, [FromBody] ApplianceDto applianceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appliance = await _applianceService.GetApplianceById(id);
            if (appliance == null || appliance.UserId != long.Parse(User.FindFirst("id")?.Value))
                return NotFound();

            appliance.ApplianceName = applianceDto.ApplianceName;
            appliance.PowerConsumption = applianceDto.PowerConsumption;

            await _applianceService.UpdateAppliance(appliance);
            return Ok();
        }

        /// <summary>
        /// Exclui um dispositivo/aparelho existente.
        /// </summary>
        /// <param name="id">ID do dispositivo a ser excluído.</param>
        /// <returns>Status de sucesso ou falha.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppliance(long id)
        {
            var appliance = await _applianceService.GetApplianceById(id);
            if (appliance == null || appliance.UserId != long.Parse(User.FindFirst("id")?.Value))
                return NotFound();

            await _applianceService.DeleteAppliance(id);
            return Ok();
        }
    }
}
