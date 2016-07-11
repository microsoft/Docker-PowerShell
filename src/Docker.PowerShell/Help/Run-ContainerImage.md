---
external help file: Docker.PowerShell.dll-help.xml
schema: 2.0.0
---

# Run-ContainerImage
## SYNOPSIS
Invoke-ContainerImage \[-Id\] \<string\[\]\> \[\[-Command\] \<string\[\]\>\] \[-RemoveAutomatically\] \[-PassThru\] \[-ContainerName \<string\>\] \[-Isolation \<IsolationType\>\] \[-Configuration \<Config\>\] \[-HostConfiguration \<HostConfig\>\] \[-Input\] \[-Terminal\] \[-HostAddress \<string\>\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]

Invoke-ContainerImage \[-Image\] \<ImagesListResponse\[\]\> \[\[-Command\] \<string\[\]\>\] \[-RemoveAutomatically\] \[-PassThru\] \[-ContainerName \<string\>\] \[-Isolation \<IsolationType\>\] \[-Configuration \<Config\>\] \[-HostConfiguration \<HostConfig\>\] \[-Input\] \[-Terminal\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]

Invoke-ContainerImage \[-Configuration\] \<Config\> \[-RemoveAutomatically\] \[-PassThru\] \[-HostConfiguration \<HostConfig\>\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]
## SYNTAX

## DESCRIPTION
Runs a new container from a specified image. 
## EXAMPLES

### Example 1
```
PS C:\> Run-Container test cmd
```

Creates and runs container test with cmd.
## PARAMETERS

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).
## INPUTS

### System.String[]
Docker.DotNet.Models.Config
Docker.DotNet.Models.ImagesListResponse[]
## OUTPUTS

### Docker.DotNet.Models.ContainerListResponse

## NOTES
This is an alias of Invoke-ContainerImage

## RELATED LINKS

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/)






