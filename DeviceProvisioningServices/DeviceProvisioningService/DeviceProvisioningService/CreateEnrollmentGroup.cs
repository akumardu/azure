using System;
using System.Collections.Generic;
using System.Text;

using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Provisioning.Service;

namespace DeviceProvisioningService
{
    public static class CreateEnrollmentGroup
    {
        public static async Task CreateIndividualEnrollment()
        {
            using (ProvisioningServiceClient provisioningServiceClient =
                    ProvisioningServiceClient.CreateFromConnectionString(Constants.ProvisioningConnectionString))
            {
                #region Create a new enrollmentGroup config
                Console.WriteLine("\nCreating a new enrollmentGroup...");
                var certificate = new X509Certificate2(Constants.X509RootCertPath);
                Attestation attestation = X509Attestation.CreateFromRootCertificates(certificate);
                EnrollmentGroup enrollmentGroup =
                        new EnrollmentGroup(
                                Constants.EnrollmentGroupId,
                                attestation)
                        {
                            ProvisioningStatus = ProvisioningStatus.Enabled
                        };
                Console.WriteLine(enrollmentGroup);
                #endregion

                #region Create the enrollmentGroup
                Console.WriteLine("\nAdding new enrollmentGroup...");
                EnrollmentGroup enrollmentGroupResult =
                    await provisioningServiceClient.CreateOrUpdateEnrollmentGroupAsync(enrollmentGroup).ConfigureAwait(false);
                Console.WriteLine("\nEnrollmentGroup created with success.");
                Console.WriteLine(enrollmentGroupResult);
                #endregion

            }
        }
    }
}
