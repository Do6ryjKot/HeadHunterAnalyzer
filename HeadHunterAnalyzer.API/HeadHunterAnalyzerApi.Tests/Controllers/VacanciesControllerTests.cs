using AutoMapper;
using Contracts.DataServices;
using Contracts.HeadHunter;
using Contracts.Logger;
using Entities.DataTransferObjects;
using Entities.Models;
using HeadHunterAnalyzer.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HeadHunterAnalyzerApi.Tests.Controllers {
	
	public class VacanciesControllerTests {

		private readonly VacanciesController _controller;
		private readonly Mock<IMapper> _mapperMoq;
		private readonly Mock<ILoggerManager> _loggerManageMoq;
		private readonly Mock<IRepositoryManager> _repositoryManagerMoq;
		private readonly Mock<IHeadHunterService> _hhService;

		public VacanciesControllerTests() {

			_mapperMoq = new Mock<IMapper>();
			_loggerManageMoq = new Mock<ILoggerManager>();
			_repositoryManagerMoq = new Mock<IRepositoryManager>();
			_hhService = new Mock<IHeadHunterService>();

			_controller = new VacanciesController(_mapperMoq.Object, 
				_loggerManageMoq.Object, 
				_repositoryManagerMoq.Object,
				_hhService.Object);
		}

		/// <summary>
		/// Если вакансия уже сохранена - возврат StatusCode 400
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task SaveAnalyzedVacancy_VacancyExists_ReturnsBadRequest() {

			// Arrange
			int headHunterId = new Random().Next(1, int.MaxValue);

			VacancyForCreationDto vacancyDto = new VacancyForCreationDto {

				HeadHunterId = headHunterId
			};

			Vacancy vacancy = new Vacancy { 
				
				HeadHunterId = headHunterId
			};

			Mock<IVacancyRepository> vacancyRepositoryMoq = new Mock<IVacancyRepository>();
			vacancyRepositoryMoq.Setup(vacancyRepository => vacancyRepository.GetVacancyAsync(headHunterId, false))
				.ReturnsAsync(vacancy);

			_repositoryManagerMoq.Setup(manager => manager.Vacancies).Returns(vacancyRepositoryMoq.Object);			
			

			// Act
			var result = await _controller.SaveAnalyzedVacancy(vacancyDto) as ObjectResult;


			// Assert
			Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
		}

		/// <summary>
		/// Если вакансии нет в сохраненных - её добавление - возврат StatusCode 200
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task SaveAnalyzedVacancy_VacancyNotExists_ReturnsOk() {

			// Arrange
			int vacancyHeadHunterId = new Random().Next(1, int.MaxValue);
			int companyHeadHunterId = new Random().Next(1, int.MaxValue);

			Mock<IVacancyRepository> vacancyRepositoryMoq = new Mock<IVacancyRepository>();
			vacancyRepositoryMoq.Setup(vacancyRepository => vacancyRepository.GetVacancyAsync(vacancyHeadHunterId, false))
				.ReturnsAsync((Vacancy?)null);

			Mock<ICompanyRepository> companyRepositoryMoq = new Mock<ICompanyRepository>();
			companyRepositoryMoq.Setup(companyRepository => companyRepository.GetCompanyByHhIdAsync(companyHeadHunterId, false))
				.ReturnsAsync(new Company());

			_repositoryManagerMoq.Setup(manager => manager.Vacancies).Returns(vacancyRepositoryMoq.Object);
			_repositoryManagerMoq.Setup(manager => manager.Companies).Returns(companyRepositoryMoq.Object);

			_hhService.Setup(hhService => hhService.GetCompany()).Returns(new Company { HeadHunterId = companyHeadHunterId });
			_hhService.Setup(hhService => hhService.GetVacancy()).Returns(new Vacancy { HeadHunterId = vacancyHeadHunterId });

			VacancyForCreationDto vacancyDto = new VacancyForCreationDto {

				HeadHunterId = vacancyHeadHunterId
			};


			// Act
			var result = await _controller.SaveAnalyzedVacancy(vacancyDto) as ObjectResult;


			// Assert
			Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
		}
	
		
	}
}
