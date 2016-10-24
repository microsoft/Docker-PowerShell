---
external help file: Docker.PowerShell.dll-Help.xml
schema: 2.0.0
---

# Get-ContainerNet
## SYNOPSIS
Gets a network endpoint.
## SYNTAX

### NetworkName (Default)
```
Get-ContainerNet [[-Name] <String[]>] [-HostAddress <String>] [-CertificateLocation <String>]
 [<CommonParameters>]
```

### Default
```
Get-ContainerNet [[-Id] <String[]>] [-HostAddress <String>] [-CertificateLocation <String>]
 [<CommonParameters>]
```

## DESCRIPTION
Gets a network endpoint.
## EXAMPLES

### Example 1
```
PS C:\> Get-ContainerNet -Name myNatNetwork
```

Gets network endpoint information for the network "myNatNetwork".
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

### -Id
Specifies the Id of the network endpoint.

```yaml
Type: String[]
Parameter Sets: Default
Aliases: 

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Name
Specifies the Name of the network endpoint.

```yaml
Type: String[]
Parameter Sets: NetworkName
Aliases: 

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).
## INPUTS

### None

## OUTPUTS

### Docker.DotNet.Models.NetworkListResponse

## NOTES

## RELATED LINKS

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/blob/master/src/Docker.PowerShell/Help/Get-ContainerNet.md)






