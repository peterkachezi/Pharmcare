using AutoMapper;
using PharmCare.DAL.Models;
using PharmCare.DTO.Accounts.BankModule;
using PharmCare.DTO.Accounts.ManufacturerPaymentModule;
using PharmCare.DTO.Accounts.OpeningBalanceModule;
using PharmCare.DTO.ApplicationUsersModule;
using PharmCare.DTO.CategoryModule;
using PharmCare.DTO.CountryModule;
using PharmCare.DTO.LeafSettingModule;
using PharmCare.DTO.MedicalConditionModule;
using PharmCare.DTO.MedicineModule;
using PharmCare.DTO.MedicineTypeModule;
using PharmCare.DTO.PatientModule;
using PharmCare.DTO.PrescriptionModule;
using PharmCare.DTO.ProductModule;
using PharmCare.DTO.ProductTypeModule;
using PharmCare.DTO.SalesModule;
using PharmCare.DTO.ShelfModule;
using PharmCare.DTO.StockModule;
using PharmCare.DTO.SupplierModule;
using PharmCare.DTO.UnitModule;

namespace PharmCare.BLL.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Patient, PatientDTO>().ReverseMap();

            CreateMap<Country, CountryDTO>().ReverseMap();

            CreateMap<Supplier, SupplierDTO>().ReverseMap();

            CreateMap<Category, CategoryDTO>().ReverseMap();

            CreateMap<Shelf, ShelfDTO>().ReverseMap();

            CreateMap<Unit, UnitDTO>().ReverseMap();

            CreateMap<LeafSetting, LeafSettingDTO>().ReverseMap();

            CreateMap<Medicine, MedicineDTO>().ReverseMap();

            CreateMap<ManufacturerPayment, ManufacturerPaymentDTO>().ReverseMap();

            CreateMap<Bank, BankDTO>().ReverseMap();

            CreateMap<OpeningBalance, OpeningBalanceDTO>().ReverseMap();

            CreateMap<GoodsReceivedNote, GoodsReceivedNoteDTO>().ReverseMap();

            CreateMap<ProductType, ProductTypeDTO>().ReverseMap();

            CreateMap<Stock, GoodsReceivedHistoryDTO>().ReverseMap();

            CreateMap<GoodsReceivedHistory, GoodsReceivedHistoryDTO>().ReverseMap();

            CreateMap<SalesDetail, SalesDetailsDTO>().ReverseMap();

            CreateMap<Sale, SalesDTO>().ReverseMap();

            CreateMap<Product, ProductDTO>().ReverseMap();

            CreateMap<MedicalCondition, MedicalConditionDTO>().ReverseMap();

            CreateMap<PrescriptionDetail, PrescriptionDetailDTO>().ReverseMap();

            CreateMap<Prescription, PrescriptionDTO>().ReverseMap();

        }
    }
}
