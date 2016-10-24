---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# Remove-Container
## SYNOPSIS
Removes a container. 
## SYNTAX

### Default (Default)
```
Remove-Container [-Force] [-ContainerIdOrName] <String[]> [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

### ContainerObject
```
Remove-Container [-Force] [-Container] <ContainerListResponse[]> [-HostAddress <String>]
 [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Removes a container. 
## EXAMPLES

### Example 1
```
PS C:\> Remove-Container -ContainerIdOrName 514e
```

Removes the container with id "514e"
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

### -Container
The container to be removed.





```yaml
Type: ContainerListResponse[]
Parameter Sets: ContainerObject
Aliases: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Force
Forces the action without prompting for confirmation. 





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

### -ContainerIdOrName
An array of names or ids of the containers to be removed.

```yaml
Type: String[]
Parameter Sets: Default
Aliases: Name, Id

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
Docker.DotNet.Models.ContainerListResponse[]
## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/blob/master/src/Docker.PowerShell/Help/Remove-Container.md)






