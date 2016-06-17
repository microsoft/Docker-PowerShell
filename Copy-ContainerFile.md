---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# Copy-ContainerFile
## SYNOPSIS
Copy-ContainerFile \[-Id\] \<string\> \[-Path\] \<string\[\]\> \[-Destination \<string\>\] \[-ToContainer\] \[-HostAddress \<string\>\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]

Copy-ContainerFile \[-Container\] \<ContainerListResponse\> \[-Path\] \<string\[\]\> \[-Destination \<string\>\] \[-ToContainer\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]
## SYNTAX

### Default
```
Copy-ContainerFile [-Path] <String[]> [-Destination <String>] [-ToContainer] [-Id] <String>
 [-HostAddress <String>] [-CertificateLocation <String>] [<CommonParameters>]
```

### ContainerObject
```
Copy-ContainerFile [-Path] <String[]> [-Destination <String>] [-ToContainer]
 [-Container] <ContainerListResponse> [-CertificateLocation <String>] [<CommonParameters>]
```

## DESCRIPTION
Copies a file into a container
## EXAMPLES

### Example 1
```
PS C:\> {{ Copy-ContainerFile -stuff }}
```

Copies a file into container
## PARAMETERS

### -CertificateLocation
Certificate location!





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

### -Container
This is the container.





```yaml
Type: ContainerListResponse
Parameter Sets: ContainerObject
Aliases: 

Required: True
Position: 0
Default value: 
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Destination
Destination for file.





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
Host address goes here





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
Id is id, and also name!





```yaml
Type: String
Parameter Sets: Default
Aliases: 

Required: True
Position: 0
Default value: 
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Path
Which path will you choose?





```yaml
Type: String[]
Parameter Sets: (All)
Aliases: 

Required: True
Position: 1
Default value: 
Accept pipeline input: False
Accept wildcard characters: False
```

### -ToContainer
The container to copy files to





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

### System.String
Docker.DotNet.Models.ContainerListResponse
## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

[Online Version:](https://github.com/Microsoft/Docker-PowerShell)






