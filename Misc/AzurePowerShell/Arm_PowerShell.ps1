Login-AzureRmAccount

# To get which resource providers support which resources use
Get-AzureRmResourceProvider | Select-Object ProviderNamespace, ResourceTypes | Sort-Object ProviderNamespace

# To get all locations on which resources exist
Get-AzureRmResourceProvider -ProviderNamespace Microsoft.Devices| Select-Object ResourceTypes, Locations | Sort-Object ResourceTypes

# REST command for above
# https://management.azure.com/subscriptions/{subscriptionid}/providers/{provider-name}?&api-version={api-version}

# To get all available API versions for a particular resource type
((Get-AzureRmResourceProvider -ProviderNamespace Microsoft.Devices).ResourceTypes | Where-Object {$_.ResourceTypeName -eq 'IotHubs'}).ApiVersions

# To get all resource groups
Get-AzureRmResourceGroup

# To get all resources
Get-AzureRmResource


