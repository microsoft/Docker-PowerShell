---
schema: 2.0.0
external help file: Docker.PowerShell.dll-Help.xml
online version: 
---

# Set-ContainerImageTag
## SYNOPSIS
{{Fill in the Synopsis}}
## SYNTAX

### Default (Default)
```
Set-ContainerImageTag [-Force] [-Repository] <String> [[-Tag] <String>] [-Id] <String[]>
 [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

### ImageObject
```
Set-ContainerImageTag [-Force] [-Repository] <String> [[-Tag] <String>] [-Image] <ImagesListResponse[]>
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
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -Force
{{Fill Force Description}}

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

### -HostAddress
{{Fill HostAddress Description}}

```yaml
Type: String
Parameter Sets: Default
Aliases: 

Required: False
Position: Named
Default value: 
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
Default value: 
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
Default value: 
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Repository
{{Fill Repository Description}}

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: True
Position: 1
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -Tag
{{Fill Tag Description}}

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 2
Default value: 
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

