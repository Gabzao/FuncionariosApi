using _02_Application.DTOs;
using _02_Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace _01_Presentation.Controllers
{
    [ApiController]
    [Route("api/funcionarios")]
    public class FuncionariosController : ControllerBase
    {
        private readonly IFuncionarioService _service;

        public FuncionariosController(IFuncionarioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna todos os funcionários cadastrados.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FuncionarioOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var funcionarios = await _service.GetAllAsync();
            return Ok(funcionarios);
        }

        /// <summary>
        /// Retorna um funcionário específico pelo Id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FuncionarioOutputDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var funcionario = await _service.GetByIdAsync(id);
                return Ok(funcionario);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Cria um novo funcionário.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(FuncionarioOutputDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] FuncionarioInputDto dto)
        {
            var criado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
        }

        /// <summary>
        /// Atualiza os dados de um funcionário existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] FuncionarioInputDto dto)
        {
            try
            {
                await _service.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Remove um funcionário pelo Id.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}