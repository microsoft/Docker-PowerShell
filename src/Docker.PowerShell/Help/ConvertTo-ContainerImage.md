---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# ConvertTo-ContainerImage
## SYNOPSIS
ConvertTo-ContainerImage \[-Id\] \<string\[\]\> \[-Repository \<string\>\] \[-Tag \<string\>\] \[-Message \<string\>\] \[-Author \<string\>\] \[-Configuration \<Config\>\] \[-HostAddress \<string\>\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]

ConvertTo-ContainerImage \[-Container\] \<ContainerListResponse\[\]\> \[-Repository \<string\>\] \[-Tag \<string\>\] \[-Message \<string\>\] \[-Author \<string\>\] \[-Configuration \<Config\>\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]
## SYNTAX

### Default
```
ConvertTo-ContainerImage [-Repository <String>] [-Tag <String>] [-Message <String>] [-Author <String>]
 [-Configuration <Config>] [-Id] <String[]> [-HostAddress <String>] [-CertificateLocation <String>]
 [<CommonParameters>]
```

### ContainerObject
```
ConvertTo-ContainerImage [-Repository <String>] [-Tag <String>] [-Message <String>] [-Author <String>]
 [-Configuration <Config>] [-Container] <ContainerListResponse[]> [-CertificateLocation <String>]
 [<CommonParameters>]
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

### -Author
{{Fill Author Description}}





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

### -Configuration
{{Fill Configuration Description}}





```yaml
Type: Config
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -Container
{{Fill Container Description}}





```yaml
Type: ContainerListResponse[]
Parameter Sets: ContainerObject
Aliases: 

Required: True
Position: 0
Default value: 
Accept pipeline input: True (ByValue)
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
Aliases: 

Required: True
Position: 0
Default value: 
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Message
{{Fill Message Description}}





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
{{Fill Repository Description}}





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

### -Tag
{{Fill Tag Description}}





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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).
## INPUTS

### System.String[]
Docker.DotNet.Models.ContainerListResponse[]
## OUTPUTS

### Docker.DotNet.Models.ImagesListResponse

## NOTES

## RELATED LINKS

[Online Version:]()






