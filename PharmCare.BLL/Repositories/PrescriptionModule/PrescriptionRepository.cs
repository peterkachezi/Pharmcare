
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PharmCare.BLL.Utils;
using PharmCare.DAL.DbContext;
using PharmCare.DAL.Models;
using PharmCare.DTO.PrescriptionModule;
using PharmCare.DTO.SalesModule;
using System.ComponentModel;
using static PharmCare.BLL.Utils.Enumerations;

namespace PharmCare.BLL.Repositories.PrescriptionModule
{
	public class PrescriptionRepository : IPrescriptionRepository
	{
		private readonly ApplicationDbContext context;

		private readonly IMapper mapper;
		public PrescriptionRepository(ApplicationDbContext context, IMapper mapper)
		{
			this.context = context;

			this.mapper = mapper;
		}
		public async Task<PrescriptionDTO> Create(PrescriptionDTO prescriptionDTO)
		{
			try
			{
				var prescriptionId = Guid.NewGuid();

				prescriptionDTO.Id = prescriptionId;

				prescriptionDTO.CreateDate = DateTime.Now;

				prescriptionDTO.MedicineDispatchStatus = 0;

				string billNo = TransactionNumber.GetNumber();

				prescriptionDTO.BillNo = billNo;

				prescriptionDTO.PaymentStatus = 0;

				var totalAmount = prescriptionDTO.PrescriptionDetailDTO.Sum(x => x.SellingPrice * x.Quantity);

				prescriptionDTO.TotalAmount = totalAmount;

				var prescription = mapper.Map<Prescription>(prescriptionDTO);

				context.Prescriptions.Add(prescription);

				var createBill = PrescriptionBill(prescriptionDTO);

				foreach (var item in prescriptionDTO.PrescriptionDetailDTO)
				{
					var pd = new PrescriptionDetail
					{
						Id = Guid.NewGuid(),

						MedicineId = item.MedicineId,

						PrescriptionId = prescriptionDTO.Id,

						Frequency = item.Frequency,

						PatientId = prescriptionDTO.PatientId,

						WhenToTake = item.WhenToTake,

						NoOfDays = item.NoOfDays,

						CreateDate = DateTime.Now,

						CreatedBy = prescriptionDTO.CreatedBy,

						MedicineDispatchStatus = 0,

						PaymentStatus = 0,

						Quantity = item.Quantity,

						BillNo = prescriptionDTO.BillNo,

						Total = item.SellingPrice * item.Quantity,
					};

					context.PrescriptionDetails.Add(pd);
				}

				await context.SaveChangesAsync();

				return prescriptionDTO;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				return null;
			}
		}
		public PrescriptionDTO PrescriptionBill(PrescriptionDTO prescriptionDTO)
		{
			try
			{
				var data = new Bill
				{
					Id = Guid.NewGuid(),

					BillNo = prescriptionDTO.BillNo,

					PrescriptionId = prescriptionDTO.Id,

					PatientId = prescriptionDTO.PatientId,

					CreateDate = DateTime.Now,

					Status = 0,

					CreatedBy = prescriptionDTO.CreatedBy,
				};

				context.Bills.Add(data);

				context.SaveChanges();

				return prescriptionDTO;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				return null;
			}
		}
		public async Task<List<PrescriptionDTO>> GetAll()
		{
			try
			{
				var prescription = (from pres in context.Prescriptions

									join patient in context.Patients on pres.PatientId equals patient.Id

									join user in context.AppUsers on pres.CreatedBy equals user.Id

									select new PrescriptionDTO
									{
										Id = pres.Id,

										TreatmentFor = pres.TreatmentFor,

										CreateDate = pres.CreateDate,

										PatientId = pres.PatientId,

										Note = pres.Note,

										BillNo = pres.BillNo,

										PaymentStatus = pres.PaymentStatus,

										MedicineDispatchStatus = pres.MedicineDispatchStatus,

										PatientName = patient.FirstName + " " + patient.LastName,

										PatientPhoneNumber = patient.PhoneNumber,

										PatientRegCode = patient.PatientNumber,

										CreatedByName = user.FirstName + " " + user.LastName,

									}).ToListAsync();

				return await prescription;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				return null;
			}
		}
		public List<PrescriptionDetailDTO> GetAllPrescriptionDetailsById(Guid Id)
		{
			try
			{
				var details = context.PrescriptionDetails.Where(x => x.PrescriptionId == Id).ToList();

				var prescription = (from pres in details

									join med in context.Medicines on pres.MedicineId equals med.Id

									join unit in context.Units on med.UnitId equals unit.Id

									join user in context.AppUsers on pres.CreatedBy equals user.Id

									select new PrescriptionDetailDTO
									{
										Id = pres.Id,

										MedicineId = pres.MedicineId,

										BillNo = pres.BillNo,

										Quantity = pres.Quantity,

										Total = pres.Total,

										PaymentStatus = pres.PaymentStatus,

										PaymentStatusDescription = GetDescription((PaymentStatus)pres.PaymentStatus),

										PrescriptionId = pres.PrescriptionId,

										Frequency = pres.Frequency + " " + "X 1",

										PatientId = pres.PatientId,

										WhenToTake = pres.WhenToTake,

										NoOfDays = pres.NoOfDays + " " + "Days",

										CreateDate = pres.CreateDate,

										MedicineName = med.Name + " " + unit.Name + " " + unit.UnitValue,

										SellingPrice = med.SellingPrice,

										MedicineDispatchStatus = pres.MedicineDispatchStatus,

										CreatedByName = user.FirstName + " " + user.LastName

									}).ToList();

				return prescription;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				return null;
			}
		}
		public async Task<PrescriptionDTO> GetPrescriptionById(Guid Id)
		{
			try
			{
				var data = await context.Prescriptions.FindAsync(Id);

				var prescription = mapper.Map<PrescriptionDTO>(data);

				return prescription;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				return null;
			}
		}
		public static string GeneratePatientNumber()
		{
			var random = new Random();
			var chars = DateTime.Now.Ticks + "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789" + DateTime.Now.Ticks;
			return new string(Enumerable.Repeat(chars, 5)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
		public async Task<PrescriptionDetailDTO> GetById(Guid Id)
		{
			try
			{
				var data = await context.PrescriptionDetails.FindAsync(Id);

				var prescription = mapper.Map<PrescriptionDetailDTO>(data);

				return prescription;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				return null;
			}
		}
		public async Task<SalesDetailsDTO> IssueMedicine(SalesDetailsDTO salesDetailsDTO)
		{
			try
			{
				string receiptNo = TransactionNumber.GetNumber();

				salesDetailsDTO.ReceiptNo = receiptNo;

				salesDetailsDTO.Id = Guid.NewGuid();

				var createSale = CreateSale(salesDetailsDTO);

				var createSaleDetails = await SaveSalesTransactionDetails(salesDetailsDTO);

				var updateStatus = await UpdateMedicineDispatchStatus(salesDetailsDTO);

				var romoveStock = RomoveFromStock(salesDetailsDTO);

				return salesDetailsDTO;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				return null;
			}
		}
		private object CreateSale(SalesDetailsDTO salesDetailsDTO)
		{
			try
			{
				var data = new Sale
				{
					Id = salesDetailsDTO.Id,

					CreateDate = DateTime.Now,

					CreatedBy = salesDetailsDTO.CreatedBy,

					AmountPaid = salesDetailsDTO.AmountPaid,

					TotalAmount = salesDetailsDTO.Total,

					ReceiptNo = salesDetailsDTO.ReceiptNo,

					Balance = salesDetailsDTO.Balance,
				};

				context.Sales.Add(data);

				context.SaveChanges();

				return salesDetailsDTO;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				return null;
			}
		}
		private object RomoveFromStock(SalesDetailsDTO salesDetailsDTO)
		{
			try
			{
				var getStock = context.Stocks.FirstOrDefault(x => x.MedicineId == salesDetailsDTO.MedicineId);

				if (getStock != null)
				{
					using (var transaction = context.Database.BeginTransaction())
					{
						var currentQuantity = getStock.Quantity;

						getStock.Quantity = currentQuantity - salesDetailsDTO.Quantity;

						transaction.Commit();
					}
					context.SaveChanges();

					return salesDetailsDTO;
				}

				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				return null;
			}
		}
		public async Task<SalesDetailsDTO> UpdateMedicineDispatchStatus(SalesDetailsDTO salesDetailsDTO)
		{

			try
			{
				var getPrescription = await context.PrescriptionDetails.FindAsync(salesDetailsDTO.PrescriptionId);

				if (getPrescription != null)
				{
					using (var transaction = context.Database.BeginTransaction())
					{
						getPrescription.MedicineDispatchStatus = 1;

						transaction.Commit();

					}
					await context.SaveChangesAsync();

					return salesDetailsDTO;
				}

				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				return null;
			}
		}
		public async Task<SalesDetailsDTO> SaveSalesTransactionDetails(SalesDetailsDTO salesDetailsDTO)
		{
			try
			{
				var data = new SalesDetail
				{
					Id = Guid.NewGuid(),

					CreateDate = DateTime.Now,

					ReceiptNo = salesDetailsDTO.ReceiptNo,

					SaleId = salesDetailsDTO.Id,

					Total = (salesDetailsDTO.Quantity) * (salesDetailsDTO.SellingPrice),

					MedicineId = salesDetailsDTO.MedicineId,

					Quantity = salesDetailsDTO.Quantity,

					CreatedBy = salesDetailsDTO.CreatedBy,

					SellingPrice = salesDetailsDTO.SellingPrice,
				};

				context.SalesDetails.Add(data);

				await context.SaveChangesAsync();

				return salesDetailsDTO;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				return null;
			}
		}
		public async Task<bool> UnDoIssueMedicine(Guid Id)
		{
			try
			{
				bool result = false;

				var getPrescription = await context.PrescriptionDetails.FindAsync(Id);

				if (getPrescription != null)
				{
					using (var transaction = context.Database.BeginTransaction())
					{
						getPrescription.MedicineDispatchStatus = 0;

						transaction.Commit();

					}
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

		private static object SyncObj = new object();

		static Dictionary<Enum, string> _enumDescriptionCache = new Dictionary<Enum, string>();
		public static string GetDescription(Enum value)
		{
			if (value == null) return string.Empty;

			lock (SyncObj)
			{
				if (!_enumDescriptionCache.ContainsKey(value))
				{
					var description = (from m in value.GetType().GetMember(value.ToString())
									   let attr = (DescriptionAttribute)m.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault()
									   select attr == null ? value.ToString() : attr.Description).FirstOrDefault();

					_enumDescriptionCache.Add(value, description);
				}
			}

			return _enumDescriptionCache[value];
		}

		public async Task<bool> Delete(Guid Id)
		{

			try
			{
				bool result = false;

				var data = await context.Prescriptions.FindAsync(Id);

				if (data != null)
				{
					context.Prescriptions.Remove(data);

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
		public async Task<bool> DeleteDetails(Guid Id)
		{

			try
			{
				bool result = false;

				var data = await context.PrescriptionDetails.FindAsync(Id);

				if (data != null)
				{
					context.PrescriptionDetails.Remove(data);

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
	}
}
