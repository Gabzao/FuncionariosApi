using _02_Application.DTOs;
using _02_Application.Interfaces;
using _04_Domain.Entities;
using _04_Domain.Interfaces;

namespace _02_Application.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _repository;

        public FuncionarioService(IFuncionarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<FuncionarioOutputDto> CreateAsync(FuncionarioInputDto dto)
        {
            var funcionario = new Funcionario
            {
                Nome = dto.Nome,
                Cargo = dto.Cargo,
                Salario = dto.Salario,
                Departamento = dto.Departamento,
                Ativo = true
            };

            await _repository.AddAsync(funcionario);
            await _repository.SaveChangesAsync();

            return MapParaOutputDto(funcionario);
        }

        public async Task<IEnumerable<FuncionarioOutputDto>> GetAllAsync()
        {
            var funcionarios = await _repository.GetAllAsync();
            return funcionarios.Select(MapParaOutputDto);
        }

        public async Task<FuncionarioOutputDto> GetByIdAsync(int id)
        {
            var funcionario = await _repository.GetByIdAsync(id);

            if (funcionario == null)
                throw new KeyNotFoundException($"Funcionário com Id {id} não foi encontrado.");

            return MapParaOutputDto(funcionario);
        }

        public async Task UpdateAsync(int id, FuncionarioInputDto dto)
        {
            var funcionario = await _repository.GetByIdAsync(id);

            if (funcionario == null)
                throw new KeyNotFoundException($"Funcionário com Id {id} não foi encontrado.");

            funcionario.Nome = dto.Nome;
            funcionario.Cargo = dto.Cargo;
            funcionario.Salario = dto.Salario;
            funcionario.Departamento = dto.Departamento;

            _repository.Update(funcionario);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var funcionario = await _repository.GetByIdAsync(id);

            if (funcionario == null)
                throw new KeyNotFoundException($"Funcionário com Id {id} não foi encontrado.");

            _repository.Delete(funcionario);
            await _repository.SaveChangesAsync();
        }

        private static FuncionarioOutputDto MapParaOutputDto(Funcionario funcionario)
        {
            return new FuncionarioOutputDto
            {
                Id = funcionario.Id,
                Nome = funcionario.Nome,
                Cargo = funcionario.Cargo,
                Salario = funcionario.Salario,
                Departamento = funcionario.Departamento,
                Ativo = funcionario.Ativo
            };
        }
    }
}