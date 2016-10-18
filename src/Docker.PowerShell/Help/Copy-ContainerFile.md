---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# Copy-ContainerFile
## SYNOPSIS
Copies a file between container and host.
## SYNTAX

### Default (Default)
```
Copy-ContainerFile [-Path] <String[]> [-Destination <String>] [-ToContainer] [-ContainerIdOrName] <String>
 [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

### ContainerObject
```
Copy-ContainerFile [-Path] <String[]> [-Destination <String>] [-ToContainer]
 [-Container] <ContainerListResponse> [-HostAddress <String>] [-CertificateLocation <String>]
 [<CommonParameters>]
```

## DESCRIPTION
Copies a file between container and host.
## EXAMPLES

### Example 1
```
PS C:\> Copy-ContainerFile -ContainerIdOrName myContainer -Path $filepathInsideContainer -Destination c:\test\
```

Copies a file located at $filepathInsideContainer out of the container to the folder "c:\test\" on the host.
### Example 2
```
PS C:\> Copy-ContainerFile -ContainerIdOrName myContainer -ToContainer -Path $filepathOnHost -Destination c:\test\
```

Copies a file located at $filepathOnHost on the host to the folder "c:\test\" in the container.
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
Specifies the container.





```yaml
Type: ContainerListResponse
Parameter Sets: ContainerObject
Aliases: 

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Destination
Destination folder for the file.





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

### -Path
The path of the file to copy.





```yaml
Type: String[]
Parameter Sets: (All)
Aliases: 

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ToContainer
Specifies that the file will be copied from the host into the container. Otherwise, the file is copied from the container to the host.




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

### -ContainerIdOrName
Specifies the container by Id or Name.

```yaml
Type: String
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

### System.String
Docker.DotNet.Models.ContainerListResponse
## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/blob/master/src/Docker.PowerShell/Help/Copy-ContainerFile.md)






