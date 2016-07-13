---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# Remove-ContainerImage
## SYNOPSIS
Remove-ContainerImage \[-Id\] \<string\[\]\> \[-Force\] \[-HostAddress \<string\>\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]

Remove-ContainerImage \[-Image\] \<ImagesListResponse\[\]\> \[-Force\] \[-CertificateLocation \<string\>\] \[\<CommonParameters\>\]
## SYNTAX

### Default (Default)
```
Remove-ContainerImage [-Force] [-Id] <String[]> [-HostAddress <String>] [-CertificateLocation <String>]
 [<CommonParameters>]
```

### ImageObject
```
Remove-ContainerImage [-Force] [-Image] <ImagesListResponse[]> [-CertificateLocation <String>]
 [<CommonParameters>]
```

## DESCRIPTION
Removes a container image. 
## EXAMPLES

### Example 1
```
PS C:\> Remove-ContainerImage -id 124dfe
```

Removes the container with id "124dfe"
## PARAMETERS

### -CertificateLocation
The location of the X509 certificate file named "key.pfx" that will be used for authentication with the server.  (Note that certificate authorization work is still in progress and this is likely to change).





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
Forces the action without prompting for confirmation. 





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
The address of the docker daemon to connect to.





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
The id of the image to be removed. 





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
The image that is to be removed. 





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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).
## INPUTS

### System.String[]
Docker.DotNet.Models.ImagesListResponse[]
## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

[Online Version:]()






