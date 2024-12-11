Steps:

Run the application in IIS express mode
Use Postman for testing

GetAll - https://localhost:44332/api/ImageManagementAPI/GetAll
GetById - https://localhost:44332/api/ImageManagementAPI/GetImageById/2
Upload (Post) - https://localhost:44332/api/ImageManagementAPI/Upload
	- Select form-data in Body
	- pass imageDetails as correct Json payload
	- pass file as 2nd param and select file to upload
	- Send
	
Sample body for imageDetails - {"User": "test", "Url": "https://test.com", "Description": "test"}
Delete (Delete) - https://localhost:44332/api/ImageManagementAPI/1

upload file location - ImageManagement.API\UploadedFiles
Image model data json file location - \ImageManagement.API\ImageData.json