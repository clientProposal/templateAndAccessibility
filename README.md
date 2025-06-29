__Before Starting__

I had an appsettings.json with my license key, you will have to make it getting yours from Apryse's site:

```json
{
  "PdfTron": {
    "LicenseKey": "demo:your-key-from-https://dev.apryse.com/"
  }
}
```

You should also run: 
dot restore && dot build 

Then:
dot run

[Get License Key](https://dev.apryse.com/)

The pdf produced at TestFiles/Output/6076.pdf is pdf/ua-1 compliant. 
The JSON has been added in the body of the class in Program.cs

