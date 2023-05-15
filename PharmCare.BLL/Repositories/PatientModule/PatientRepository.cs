

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.BLL.Utils;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.PatientModule;
using SkiaSharp;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;


namespace PharmCare.BLL.Repositories.PatientModule
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IMapper mapper;
        public PatientRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;

            this.mapper = mapper;
        }
        public async Task<PatientDTO> Create(PatientDTO patientDTO)
        {
            try
            {
                string patient_number = PatientNumber.GenerateUniqueNumber();

                patientDTO.PatientNumber = "P" + "" + patient_number;

                patientDTO.Id = Guid.NewGuid();

                patientDTO.CreateDate = DateTime.Now;

                var patient = mapper.Map<Patient>(patientDTO);

                context.Patients.Add(patient);

                await context.SaveChangesAsync();

                return patientDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<bool> Delete(Guid Id)
        {
            try
            {
                bool result = false;

                var patient = await context.Patients.FindAsync(Id);

                if (patient != null)
                {
                    context.Patients.Remove(patient);

                    await context.SaveChangesAsync();

                    return true;
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
        public async Task<PatientDTO> Update(PatientDTO patientDTO)
        {
            try
            {
                var patient = await context.Patients.FindAsync(patientDTO.Id);

                if (patient != null)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        patient.FirstName = patientDTO.FirstName;

                        patient.LastName = patientDTO.LastName;

                        patient.DateOfBirth = patientDTO.DateOfBirth;

                        patient.PhoneNumber = patientDTO.PhoneNumber;

                        patient.Gender = patientDTO.Gender;

                        patient.Residence = patientDTO.Residence;

                        transaction.Commit();
                    }
                    await context.SaveChangesAsync();

                    return patientDTO;
                }

                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<List<PatientDTO>> GetAll()
        {
            try
            {
                var data = await context.Patients.OrderByDescending(c => c.CreateDate).ToListAsync();

                var patients = mapper.Map<List<PatientDTO>>(data);

                return patients;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public async Task<PatientDTO> GetById(Guid Id)
        {
            try
            {
                var data = await context.Patients.FindAsync(Id);

                var customers = mapper.Map<PatientDTO>(data);

                return customers;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<PatientDTO> GetByPhoneNumber(string PhoneNumber)
        {
            try
            {
                var data = await context.Patients.FirstOrDefaultAsync(x => x.PhoneNumber == PhoneNumber);

                var customers = mapper.Map<PatientDTO>(data);

                return customers;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }


        public async Task<bool> CheckIfPatientExist(PatientDTO patientDTO)
        {
            try
            {
                bool result = await context.Patients.AnyAsync(p => p.PhoneNumber == patientDTO.PhoneNumber);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }



    }
}
