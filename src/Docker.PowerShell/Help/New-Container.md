---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# New-Container
## SYNOPSIS
Creates a new container.
## SYNTAX

### Default (Default)
```
New-Container [-Name <String>] [[-Command] <String[]>] [-Isolation <IsolationType>] [-Configuration <Config>]
 [-HostConfiguration <HostConfig>] [-Input] [-Terminal] [-ImageIdOrName] <String[]> [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

### ImageObject
```
New-Container [-Name <String>] [[-Command] <String[]>] [-Isolation <IsolationType>] [-Configuration <Config>]
 [-HostConfiguration <HostConfig>] [-Input] [-Terminal] [-Image] <ImagesListResponse[]> [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

### ConfigObject
```
New-Container -Configuration <Config> [-HostConfiguration <HostConfig>] [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Creates a new container.
## EXAMPLES

### Example 1
```
PS C:\> New-Container -ImageIdOrName 4179ed
```

Creates a new container from the image with id "4179ed".
## PARAMETERS

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

### -Command
The command to be run in the new container. 





```yaml
Type: String[]
Parameter Sets: Default, ImageObject
Aliases: 

Required: False
Position: 1
Default value: None
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
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

```yaml
Type: Config
Parameter Sets: ConfigObject
Aliases: 

Required: True
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

### -HostConfiguration
A Docker.DotNet.Models.HostConfig object filled in with desired configuration values.





```yaml
Type: HostConfig
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Image
The image to create the new container from.





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

### -Input
Indicates that the stdin of the container should be kept open.





```yaml
Type: SwitchParameter
Parameter Sets: Default, ImageObject
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Isolation
Indicates whether the container should be isolated as a Hyper-V Container. Available options are: HyperV, None, or Default.





```yaml
Type: IsolationType
Parameter Sets: Default, ImageObject
Aliases: 
Accepted values: Default, None, HyperV

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
The name for the new container.

```yaml
Type: String
Parameter Sets: Default, ImageObject
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Terminal
Enables terminal emulation in the new container.





```yaml
Type: SwitchParameter
Parameter Sets: Default, ImageObject
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ImageIdOrName
The Name or Id for the image to create the new container from.

```yaml
Type: String[]
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

### System.String[]
Docker.DotNet.Models.Config
Docker.DotNet.Models.ImagesListResponse[]
## OUTPUTS

### Docker.DotNet.Models.ContainerListResponse

## NOTES

## RELATED LINKS

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/blob/master/src/Docker.PowerShell/Help/New-Container.md)






