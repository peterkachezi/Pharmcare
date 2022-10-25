using PharmCare.DTO.PatientModule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PharmCare.BLL.Repositories.PatientModule
{
    public interface IPatientRepository
    {
        Task<PatientDTO> Create(PatientDTO customerDTO);
        Task<PatientDTO> Update(PatientDTO customerDTO);
        Task<bool> Delete(Guid Id);
        Task<List<PatientDTO>> GetAll();
        Task<PatientDTO> GetById(Guid Id);
        Task<PatientDTO> GetByPhoneNumber(string PhoneNumber);
        Task<bool> CheckIfPatientExist(PatientDTO patientDTO);
    }
}
