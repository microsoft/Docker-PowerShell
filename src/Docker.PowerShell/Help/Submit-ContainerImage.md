---
external help file: Docker.PowerShell.dll-Help.xml
online version: https://github.com/Microsoft/Docker-PowerShell/
schema: 2.0.0
---

# Submit-ContainerImage
## SYNOPSIS
Submits the container image by pushing it to a Docker registry.
## SYNTAX

### Default (Default)
```
Submit-ContainerImage [-Id] <String> [-PassThru] [-Authorization <AuthConfig>] [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

### ImageObject
```
Submit-ContainerImage [-Image] <ImagesListResponse> [-PassThru] [-Authorization <AuthConfig>]
 [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Submits the container image by pushing it to a Docker registry.
## EXAMPLES

### Example 1
```
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}
## PARAMETERS

### -Authorization
A Docker.DotNet.Models.AuthConfig object containing authentication information for the connection to the Docker registry being pushed to.

```yaml
Type: AuthConfig
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -CertificateLocation
The location of the X509 certificate file named "key.pfx" that will be used for authentication with the server.  (Note that certificate authorization work is still in progress and this is likely to change).

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -HostAddress
The address of the docker daemon to connect to.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
The id of the container image.

```yaml
Type: String
Parameter Sets: Default
Aliases: ImageName

Required: True
Position: 0
Default value: 
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Image
Specifies the container image object.

```yaml
Type: ImagesListResponse
Parameter Sets: ImageObject
Aliases: 

Required: True
Position: 0
Default value: 
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -PassThru
If provided, the cmdlet will return the container image object that was submitted.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).
## INPUTS

### System.String
Docker.DotNet.Models.ImagesListResponse
## OUTPUTS

### Docker.DotNet.Models.ContainerListResponse

## NOTES

## RELATED LINKS

