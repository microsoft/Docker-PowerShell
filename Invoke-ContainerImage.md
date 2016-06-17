---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# Invoke-ContainerImage
## SYNOPSIS
Invoke-ContainerImage \[-Id\] \<string\[\]\> \[\[-Command\] \<string\[\]\>\] \[-RemoveAutomatically\] \[-PassThru\] \[-ContainerName \<string\>\] \[-Isolation \<IsolationType\>\] \[-Configuration \<Config\>\] \[-HostConfiguration \<HostConfig\>\] \[-Input\] \[-Terminal\] \[-HostAddress \<string\>\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]

Invoke-ContainerImage \[-Image\] \<ImagesListResponse\[\]\> \[\[-Command\] \<string\[\]\>\] \[-RemoveAutomatically\] \[-PassThru\] \[-ContainerName \<string\>\] \[-Isolation \<IsolationType\>\] \[-Configuration \<Config\>\] \[-HostConfiguration \<HostConfig\>\] \[-Input\] \[-Terminal\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]

Invoke-ContainerImage \[-Configuration\] \<Config\> \[-RemoveAutomatically\] \[-PassThru\] \[-HostConfiguration \<HostConfig\>\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]
## SYNTAX

### Default
```
Invoke-ContainerImage [-RemoveAutomatically] [-PassThru] [-ContainerName <String>] [[-Command] <String[]>]
 [-Isolation <IsolationType>] [-Configuration <Config>] [-HostConfiguration <HostConfig>] [-Input] [-Terminal]
 [-Id] <String[]> [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

### ImageObject
```
Invoke-ContainerImage [-RemoveAutomatically] [-PassThru] [-ContainerName <String>] [[-Command] <String[]>]
 [-Isolation <IsolationType>] [-Configuration <Config>] [-HostConfiguration <HostConfig>] [-Input] [-Terminal]
 [-Image] <ImagesListResponse[]> [-CertificateLocation <String>] [<CommonParameters>]
```

### ConfigObject
```
Invoke-ContainerImage [-RemoveAutomatically] [-PassThru] -Configuration <Config>
 [-HostConfiguration <HostConfig>] [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Runs a container from an existing image.
## EXAMPLES

### Example 1
```
PS C:\> Invoke-ContainerImage -id 903ef
```

Starts a container using the image with id 903ef.
## PARAMETERS

### -CertificateLocation
Specifies the certificate location. 





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

### -Command
{{Fill Command Description}}





```yaml
Type: String[]
Parameter Sets: Default, ImageObject
Aliases: 

Required: False
Position: 1
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -Configuration
{{Fill Configuration Description}}





```yaml
Type: Config
Parameter Sets: Default, ImageObject
Aliases: 

Required: False
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

```yaml
Type: Config
Parameter Sets: ConfigObject
Aliases: 

Required: True
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -ContainerName
{{Fill ContainerName Description}}





```yaml
Type: String
Parameter Sets: Default, ImageObject
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

### -HostConfiguration
{{Fill HostConfiguration Description}}





```yaml
Type: HostConfig
Parameter Sets: (All)
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

### -Input
{{Fill Input Description}}





```yaml
Type: SwitchParameter
Parameter Sets: Default, ImageObject
Aliases: 

Required: False
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -Isolation
{{Fill Isolation Description}}





```yaml
Type: IsolationType
Parameter Sets: Default, ImageObject
Aliases: 
Accepted values: Default, None, HyperV

Required: False
Position: Named
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
{{Fill PassThru Description}}





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

### -RemoveAutomatically
{{Fill RemoveAutomatically Description}}





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

### -Terminal
{{Fill Terminal Description}}





```yaml
Type: SwitchParameter
Parameter Sets: Default, ImageObject
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
Docker.DotNet.Models.Config
Docker.DotNet.Models.ImagesListResponse[]
## OUTPUTS

### Docker.DotNet.Models.ContainerListResponse

## NOTES

## RELATED LINKS

[Online Version:]()






