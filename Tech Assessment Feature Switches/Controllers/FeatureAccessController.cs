using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tech_Assessment_Feature_Switches.Models;

namespace Tech_Assessment_Feature_Switches.Controllers
{
    
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureAccessController : ControllerBase
    {
        private readonly FeatureAccessContext _context;

        public FeatureAccessController(FeatureAccessContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns the full record from the database.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeatureAccess>>> GetFeatureAccess()
        {
            return await _context.FeatureAccess.ToListAsync();
        }

        /// <summary>
        /// Returns whether a feature is enabled for a user. 
        /// </summary>
        /// <remarks>
        /// Attempt to retrieve the user and feature name mapping record from the database.
        /// 
        /// If mapping record exists, return whether the user has access to the feature.
        /// 
        /// If mapping record does not exists, return message indicating that user to feature mapping does not exist within the database.
        /// 
        /// Sample request:
        ///
        ///     GET /feature??email=testUser2&amp;featureName=Export
        ///
        /// </remarks>
        /// <response code="200">Returns the users access right to the feature.</response>
        /// <response code="400">Invalid request. Read message returned for more details.</response>
        [HttpGet("feature")]
        public async Task<ActionResult<FeatureAccess>> GetFeatureAccess([FromQuery(Name = "email")]string email, [FromQuery(Name = "featureName")]string featureName)
        {
            bool errorDetected = false;
            string responseMessage = string.Empty;
            bool canAccess = false;
            if (string.IsNullOrEmpty(email)){
                errorDetected = true;
                responseMessage = string.Format("{0}{1}", responseMessage, "No email supplied to request. Please provide an email parameter.");
            }

            if (string.IsNullOrEmpty(featureName)){
                errorDetected = true;
                responseMessage = string.Format("{0}{1}", responseMessage, "No feature name supplied to request. Please provide a feature name parameter.");
            }

            if (errorDetected){
                return BadRequest(responseMessage);
            }

            var featureAccess = await _context.FeatureAccess.FindAsync(email, featureName);
            
            if (featureAccess != null){
                try{
                    canAccess = (bool)featureAccess.Enable;
                    var output = new {
                        canAccess = canAccess
                    };
                    return Ok(output);
                }
                catch (Exception ex){
                    errorDetected = true;
                    responseMessage = string.Format("{0}{1}", responseMessage, "Mapping of user to feature name supplied does not exist within the database.");
                }

            }
            else{
                errorDetected = true;
                responseMessage = string.Format("{0}{1}", responseMessage, "Mapping of user to feature name supplied does not exist within the database.");
            }

            return BadRequest(responseMessage);
        }

        /// <summary>
        /// Manages a users access to features. 
        /// </summary>
        /// <remarks>
        /// Attempt to update the 'Enable' property for a user to feature mapping.
        /// 
        /// If mapping record exists and the 'Enable' paramameter from the POST body does not match with the database record, update the database record 'Enable' column to the incoming 'Enable' parameter and return status code 200.
        /// 
        /// If mapping record exists and the 'Enable' paramameter from the POST body matches with the database record, return status code 304 (Not Modified).
        /// 
        /// If mapping record does not exist, return status code 304 (Not Modified).
        /// 
        /// Sample request:
        ///
        ///     POST /feature
        ///     {
        ///         "featureName": "Export",
        ///         "email": "testUser2",
        ///         "enable": true
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Successfully updated</response>
        /// <response code="304">Not Modified</response>
        [HttpPost("feature")]
        public async Task<ActionResult<FeatureAccess>> UpdateUserFeatureAccess([FromBody]FeatureAccess request)
        {
            bool errorDetected = false;
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.FeatureName) || request.Enable == null){
                errorDetected = true;
            }

            if (!string.IsNullOrEmpty(request.Email) && !string.IsNullOrEmpty(request.FeatureName) && request.Enable != null){
                var featureAccess = await _context.FeatureAccess.FindAsync(request.Email, request.FeatureName);

                if (featureAccess != null){
                    if (featureAccess.Enable == request.Enable){
                        errorDetected = true;
                    }
                    else{
                        featureAccess.Enable = request.Enable;
                    }

                    _context.FeatureAccess.Update(featureAccess);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        errorDetected = true;
                    }
                }   
                else{
                    errorDetected = true;
                }
            }

            if (errorDetected){
                return StatusCode(304);
            }
            else{
                return Ok("Successfully updated the database.");
            }
        }
    }
}
