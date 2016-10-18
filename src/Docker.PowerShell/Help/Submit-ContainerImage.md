---
external help file: Docker.PowerShell.dll-Help.xml
online version: https://github.com/Microsoft/Docker-PowerShell/
schema: 2.0.0
---

# Submit-ContainerImage
## SYNOPSIS
Submits the container image by pushing it to a Docker registry.
Aliased as "Push-ContainerImage".
## SYNTAX

### Default (Default)
```
Submit-ContainerImage [-ImageIdOrName] <String> [-PassThru] [-Authorization <AuthConfig>]
 [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

### ImageObject
```
Submit-ContainerImage [-Image] <ImagesListResponse> [-PassThru] [-Authorization <AuthConfig>]
 [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Submits the container image by pushing it to a Docker registry.
Aliased as "Push-ContainerImage".
## EXAMPLES

### Example 1
```
PS C:\> Submit-ContainerImage -ImageIdOrName myImage
```

Pushes the image named "myImage" to the Docker registry.
## PARAMETERS

### -Authorization
A Docker.DotNet.Models.AuthConfig object containing authentication information for the connection to the Docker registry being pushed to.

```yaml
Type: AuthConfig
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
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
Default value: None
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
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Image
Specifies the container image object to be pushed.

```yaml
Type: ImagesListResponse
Parameter Sets: ImageObject
Aliases: 

Required: True
Position: 0
Default value: None
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
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ImageIdOrName
The Id or Name of the image to be pushed.

```yaml
Type: String
Parameter Sets: Default
Aliases: ImageName, ImageId

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
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

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/blob/master/src/Docker.PowerShell/Help/Submit-ContainerImage.md)
