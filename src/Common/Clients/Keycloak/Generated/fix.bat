powershell -Command "(gc KeycloakGeneratedExternalApiClient.cs) -replace 'status_ == 200', 'response_.IsSuccessStatusCode' | Out-File KeycloakGeneratedExternalApiClient.cs"
pause