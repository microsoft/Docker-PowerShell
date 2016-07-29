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

### Default (Default)
```
Invoke-ContainerImage [-RemoveAutomatically] [-PassThru] [-Detach] [-Name <String>] [[-Command] <String[]>]
 [-Isolation <IsolationType>] [-Configuration <Config>] [-HostConfiguration <HostConfig>] [-Input] [-Terminal]
 [-Id] <String[]> [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

### ImageObject
```
Invoke-ContainerImage [-RemoveAutomatically] [-PassThru] [-Detach] [-Name <String>] [[-Command] <String[]>]
 [-Isolation <IsolationType>] [-Configuration <Config>] [-HostConfiguration <HostConfig>] [-Input] [-Terminal]
 [-Image] <ImagesListResponse[]> [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

### ConfigObject
```
Invoke-ContainerImage [-RemoveAutomatically] [-PassThru] [-Detach] -Configuration <Config>
 [-HostConfiguration <HostConfig>] [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
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
Specifies the command to run in the container.





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
An instance of a Docker.DotNet.Models.Config object with desired configuration values filled out.





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

### -HostConfiguration
A Docker.DotNet.Models.HostConfig object filled in with desired configuration values.





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
The id of the container image.





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
Specifies the container image.





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
Indicates that the stdin of the container should be kept open.





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
Specifies whether the container should be run as a Hyper-V Container. Options are HyperV, None, or Default.





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

### -Name
{{Fill Name Description}}

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

### -PassThru
Passes the object through the pipeline.





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
Automatically removes the container after running.





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
Gives a terminal in the new container.





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

### -Detach
Run the container in detached mode, which skips attaching to input/output pipes.

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

### System.String[]
Docker.DotNet.Models.Config
Docker.DotNet.Models.ImagesListResponse[]
## OUTPUTS

### Docker.DotNet.Models.ContainerListResponse

## NOTES

## RELATED LINKS

[Online Version:]()






