using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace HeadHunterAnalyzer.API.Mapping {
	
	public class MappingProfiles : Profile {

		public MappingProfiles() {

			CreateMap<VacancyForCreationDto, Vacancy>();
			CreateMap<WordForCreationDto, Word>();

			CreateMap<Company, AnalyzedCompanyDto>();
			CreateMap<Word, WordDto>();

			CreateMap<Vacancy, VacancyDto>();
		}
	}
}
