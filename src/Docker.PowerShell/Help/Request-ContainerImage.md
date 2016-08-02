---
external help file: Docker.PowerShell.dll-Help.xml
online version: 
schema: 2.0.0
---

# Request-ContainerImage
## SYNOPSIS
Downloads a container image matching the given repository and tag from the Docker registry.
## SYNTAX

```
Request-ContainerImage [-Repository] <String> [[-Tag] <String>] [-All] [-Authorization <AuthConfig>]
 [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Downloads a container image matching the given repository and tag from the Docker registry.
## EXAMPLES

### Example 1
```
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}
## PARAMETERS

### -All
If specified, Tag is ignored and all images matching the given Repository are downloaded into the Docker daemon.

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

### -Repository
The repository of the desired image.  This may include a registry address from which the image should be downloaded. 

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: True
Position: 0
Default value: 
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Tag
The tag for which image to download.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 1
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).
## INPUTS

### System.String

## OUTPUTS

### Docker.DotNet.Models.ContainerListResponse

## NOTES

## RELATED LINKS

