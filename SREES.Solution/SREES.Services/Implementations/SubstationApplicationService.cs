using Microsoft.Extensions.Logging;
using SREES.BLL.Services.Interfaces;
using SREES.Common.Models;
using SREES.Common.Models.Dtos.Statistics;
using SREES.Common.Models.Dtos.Substations;
using SREES.Services.Interfaces;

namespace SREES.Services.Implementations
{
    public class SubstationApplicationService : ISubstationApplicationService
    {
        private readonly ILogger<SubstationApplicationService> _logger;
        private readonly ISubstationService _substationService;

        public SubstationApplicationService(ILogger<SubstationApplicationService> logger, ISubstationService substationService)
        {
            _logger = logger;
            _substationService = substationService;
        }

        public async Task<ResponsePackage<List<SubstationDataOut>>> GetAllSubstations()
        {
            return await _substationService.GetAllSubstations();
        }

        public async Task<ResponsePackage<List<SubstationSelectDataOut>>> GetAllSubstationsForSelect()
        {
            return await _substationService.GetAllSubstationsForSelect();
        }

        public async Task<ResponsePackage<SubstationDataOut?>> GetSubstationById(int id)
        {
            return await _substationService.GetSubstationById(id);
        }

        public async Task<ResponsePackage<SubstationDataOut?>> CreateSubstation(SubstationDataIn substationDataIn)
        {
            return await _substationService.CreateSubstation(substationDataIn);
        }

        public async Task<ResponsePackage<SubstationDataOut?>> UpdateSubstation(int id, SubstationDataIn substationDataIn)
        {
            return await _substationService.UpdateSubstation(id, substationDataIn);
        }

        public async Task<ResponsePackage<string>> DeleteSubstation(int id)
        {
            return await _substationService.DeleteSubstation(id);
        }

        public async Task<ResponsePackage<List<EntityCountStatisticsDataOut>>> GetSubstationStatistics()
        {
            return await _substationService.GetSubstationStatistics();
        }
    }
}
