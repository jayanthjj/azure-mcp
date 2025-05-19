targetScope = 'resourceGroup'

@description('The base resource name')
param baseName string = resourceGroup().name

@description('The location for the Datadog monitor')
param location string = resourceGroup().location

@description('The tenant ID to which the application and resources belong.')
param tenantId string = '72f988bf-86f1-41af-91ab-2d7cd011db47'

@description('The client OID to grant access')
param testApplicationOid string

resource datadogMonitor 'Microsoft.Datadog/monitors@2022-06-01' = {
  name: baseName
  location: location
  properties: {
    monitoringStatus: 'Enabled'
    marketplaceSubscriptionStatus: 'Subscribed'
    datadogOrganizationProperties: {
      apiKey: 'datadogApiKey'
      applicationKey: 'datadogAppKey'
      enterpriseAppId: '00000000-0000-0000-0000-000000000000' // Replace with actual ID if needed
    }
    datadogRegion: 'datadogSite'
    singleSignOnProperties: {
      singleSignOnState: 'Enabled'
      enterpriseAppId: '00000000-0000-0000-0000-000000000000' // Replace with actual ID if needed
    }
    tags: {
      environment: 'test'
    }
  }
}

resource datadogContributorRole 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: subscription()
  name: 'f9a17ca0-28a9-41f9-b9d3-b3e8e9e2aecd' // Datadog Contributor role
}

resource datadogRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(testApplicationOid, datadogContributorRole.id, datadogMonitor.id)
  scope: datadogMonitor
  properties: {
    principalId: testApplicationOid
    roleDefinitionId: datadogContributorRole.id
  }
}
