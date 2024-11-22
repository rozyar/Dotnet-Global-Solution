namespace SolarPanelCalculatorApi.Presentation.Controllers
{
    using System.Text.Json;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using SolarPanelCalculatorApi.Application.DTO;
    using SolarPanelCalculatorApi.Domain.Interfaces;
    using SolarPanelCalculatorApi.Domain.Models;



    /// <summary>
    /// Controller para gerenciar análises de energia solar.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AnalysesController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;
        private readonly IApplianceService _applianceService;
        private readonly IMapper _mapper;

        public AnalysesController(IAnalysisService analysisService, IApplianceService applianceService, IMapper mapper)
        {
            _analysisService = analysisService;
            _applianceService = applianceService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém todas as análises do usuário autenticado.
        /// </summary>
        /// <returns>Lista de análises do usuário com os eletrodomésticos utilizados.</returns>
        [HttpGet]
        public async Task<IActionResult> GetUserAnalyses()
        {
            var userId = long.Parse(User.FindFirst("id")?.Value);
            var analyses = await _analysisService.GetAnalysesByUserId(userId);

            var analysesDto = analyses.Select(analysis =>
            {
                var analysisDto = _mapper.Map<AnalysisDto>(analysis);

                // Desserializar o JSON dos eletrodomésticos
                if (!string.IsNullOrEmpty(analysis.AppliancesJson))
                {
                    analysisDto.Appliances = JsonSerializer.Deserialize<List<ApplianceDto>>(analysis.AppliancesJson);
                }

                return analysisDto;
            });

            return Ok(analysesDto);
        }

        /// <summary>
        /// Obtém análises do usuário autenticado de forma paginada.
        /// </summary>
        /// <param name="page">Número da página (começa em 0).</param>
        /// <param name="pageSize">Número de itens por página.</param>
        /// <returns>Análises do usuário paginadas com os eletrodomésticos utilizados.</returns>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedUserAnalyses(
            [FromQuery] int page = 0, [FromQuery] int pageSize = 5)
        {
            var userId = long.Parse(User.FindFirst("id")?.Value);
            var analyses = await _analysisService.GetPagedAnalysesByUserId(userId, page, pageSize);

            var analysesDto = analyses.Select(analysis =>
            {
                var analysisDto = _mapper.Map<AnalysisDto>(analysis);

                // Desserializar o JSON dos eletrodomésticos
                if (!string.IsNullOrEmpty(analysis.AppliancesJson))
                {
                    analysisDto.Appliances = JsonSerializer.Deserialize<List<ApplianceDto>>(analysis.AppliancesJson);
                }

                return analysisDto;
            });

            return Ok(new { currentPage = page, pageSize, items = analysesDto });
        }

        /// <summary>
        /// Cria uma nova análise de energia solar.
        /// </summary>
        /// <param name="analysisDto">Dados da análise.</param>
        /// <returns>Resultado da análise criada.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAnalysis([FromBody] AnalysisDto analysisDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //testes
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                Console.WriteLine("User ID not found in token."); // Log básico para depuração
                return Unauthorized(new { message = "User ID not found in token." });
            }

            var userId = long.Parse(userIdClaim);
            Console.WriteLine($"Extracted User ID: {userId}"); // Log do valor de userId
            var appliances = await _applianceService.GetAppliancesByUserId(userId);

            if (!appliances.Any())
            {
                return BadRequest(new { message = "You need to add appliances before calculating." });
            }

            if (analysisDto.SunlightHours <= 0 || analysisDto.SunlightHours > 24)
            {
                return BadRequest(new { message = "Sunlight hours must be between 1 and 24." });
            }

            var analysis = _mapper.Map<Analysis>(analysisDto);
            analysis.UserId = userId;

            try
            {
                var result = await _analysisService.CreateAnalysis(analysis, appliances);
                var resultDto = _mapper.Map<AnalysisDto>(result);

                resultDto.Appliances = JsonSerializer.Deserialize<List<ApplianceDto>>(result.AppliancesJson);
                return Ok(resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Exclui uma análise específica.
        /// </summary>
        /// <param name="id">ID da análise a ser excluída.</param>
        /// <returns>Confirmação da exclusão.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnalysis(long id)
        {
            var analysis = await _analysisService.GetAnalysisById(id);
            if (analysis == null || analysis.UserId != long.Parse(User.FindFirst("id")?.Value))
                return NotFound();

            await _analysisService.DeleteAnalysis(id);
            return Ok();
        }
    }
}