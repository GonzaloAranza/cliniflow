using CliniFlow.Application.DTOs;
using CliniFlow.Application.Interfaces;
using CliniFlow.Domain.Entities;
using CliniFlow.Domain.Enums;

namespace CliniFlow.Application.Services;

public class ProfessionalService : IProfessionalService
{
    private readonly IUserRepository _userRepository;
    private readonly IProfessionalRepository _professionalRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProfessionalService(
        IUserRepository userRepository,
        IProfessionalRepository professionalRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _professionalRepository = professionalRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProfessionalDto>> GetAllAsync()
    {
        var professionals = await _professionalRepository.GetAllAsync();

        // Mapeo manual de Entidad -> DTO simple
        return professionals.Select(p => new ProfessionalDto
        {
            Id = p.Id,
            // Concatenamos nombre y apellido del Usuario relacionado
            FullName = $"{p.User.LastName}, {p.User.FirstName}",
            Specialty = p.Specialty ?? "General",
            LicenseNumber = p.LicenseNumber
        });
    }

    public async Task<ProfessionalDetailDto?> GetByIdAsync(int id)
    {
        var p = await _professionalRepository.GetByIdAsync(id);
        if (p == null) return null;

        // AQUÍ ESTÁ LA CLAVE: 
        // Extraemos datos de 'p.User' y los metemos en el DTO
        return new ProfessionalDetailDto
        {
            Id = p.Id,
            UserId = p.UserId,
            // Datos de User
            FirstName = p.User.FirstName,
            LastName = p.User.LastName,
            Email = p.User.Email,
            // DNI = p.User.DNI, // Descomenta si tu User tiene DNI

            // Datos de Professional
            LicenseNumber = p.LicenseNumber,
            Specialty = p.Specialty ?? "General",
            Phone = p.Phone,
            IsActive = p.IsActive
        };
    }

    public async Task<int> RegisterProfessionalAsync(CreateProfessionalDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {

            var userWithDni = await _userRepository.GetByDNIAsync(dto.DNI);
            if (userWithDni != null)
                throw new InvalidOperationException($"El DNI {dto.DNI} ya está registrado.");
            // 1. Validaciones previas...
            // ... (validar email y matrícula)

            // IMPORTANTE: Deberías validar también si el DNI ya existe
            // var existingDNI = await _userRepository.GetByDNIAsync(dto.DNI);
            // if (existingDNI != null) throw ...

            // 2. Crear Usuario
            var newUser = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DNI = dto.DNI, // <--- ¡ASIGNACIÓN AGREGADA!
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = Role.Professional,
                IsActive = true
            };

            var createdUser = await _userRepository.CreateAsync(newUser);

            // Crear Profesional
            var newProfessional = new Professional
            {
                UserId = createdUser.Id,
                LicenseNumber = dto.LicenseNumber,
                Specialty = dto.Specialty,
                Phone = dto.Phone,
                IsActive = true
            };

            await _professionalRepository.AddAsync(newProfessional);
            await _unitOfWork.CommitAsync();

            return newProfessional.Id;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateAsync(int id, UpdateProfessionalDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var professional = await _professionalRepository.GetByIdAsync(id);
            if (professional == null)
                throw new KeyNotFoundException($"Profesional {id} no encontrado");

            // 1. Actualizamos datos en la tabla Users
            professional.User.FirstName = dto.FirstName;
            professional.User.LastName = dto.LastName;
            // Ojo: Si permites cambiar email, valida duplicados aquí

            await _userRepository.UpdateAsync(professional.User);

            // 2. Actualizamos datos en la tabla Professionals
            professional.Phone = dto.Phone;
            professional.Specialty = dto.Specialty;

            await _professionalRepository.UpdateAsync(professional);

            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        await _professionalRepository.DeleteAsync(id);
        // Nota: UnitOfWork no es estrictamente necesario aquí si el repo ya guarda,
        // pero es buena práctica usarlo para consistencia si cambiamos la lógica del repo.
    }
}