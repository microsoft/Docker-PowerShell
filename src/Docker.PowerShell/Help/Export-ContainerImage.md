---
external help file: Docker.PowerShell.dll-Help.xml
online version: https://github.com/Microsoft/Docker-PowerShell
schema: 2.0.0
---

# Export-ContainerImage
## SYNOPSIS
{{Fill in the Synopsis}}
## SYNTAX

### Default (Default)
```
Export-ContainerImage -DestinationFilePath <String> [-Id] <String[]> [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

### ImageObject
```
Export-ContainerImage -DestinationFilePath <String> [-Image] <ImagesListResponse[]> [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
{{Fill in the Description}}
## EXAMPLES

### Example 1
```
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}
## PARAMETERS

### -CertificateLocation
{{Fill CertificateLocation Description}}

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
{{Fill HostAddress Description}}

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

### -Id
{{Fill Id Description}}

```yaml
Type: String[]
Parameter Sets: Default
Aliases: ImageName

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Image
{{Fill Image Description}}

```yaml
Type: ImagesListResponse[]
Parameter Sets: ImageObject
Aliases: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -DestinationFilePath
{{Fill DestinationFilePath Description}}

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).
## INPUTS

### System.String[]
Docker.DotNet.Models.ImagesListResponse[]
## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

