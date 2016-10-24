---
external help file: Docker.PowerShell.dll-Help.xml
online version: 
schema: 2.0.0
---

# New-ContainerNet
## SYNOPSIS
Creates a new network.
## SYNTAX

```
New-ContainerNet [-HostAddress <String>] [-CertificateLocation <String>] [[-Name] <String>]
 [[-Driver] <String>] [-Internal] [-CheckDuplicate] [-EnableIPv6] [-IPAM <IPAM>]
 [-Options <System.Collections.Generic.IDictionary`2[System.String,System.String]>]
 [-Labels <System.Collections.Generic.IDictionary`2[System.String,System.String]>] [<CommonParameters>]
```

## DESCRIPTION
Creates a new network.
## EXAMPLES

### Example 1
```
PS C:\> New-ContainerNet -Name myNet -Driver transparent
```

Creates a new transparent network endpoing called "myNet".
### Example 2
```
PS C:\> $opt = New-Object 'System.Collections.Generic.Dictionary[String,String]'
PS C:\> $opt.add("com.docker.network.windowsshim.interface","Virtual Switch")
PS C:\> New-ContainerNet -Name externalNet -Driver transparent -Options $opt
```

Creates a new transparent network called "externalNet" that is connected to the
Hyper-V switch named "Virtual Switch".
## PARAMETERS

### -CertificateLocation
The location of the X509 certificate file named "key.pfx" that will be used for authentication with the server. (Note that certificate authorization work is still in progress and this is likely to change).

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

### -CheckDuplicate
Requests that the daemon check for networks with the same name.

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

### -Driver
The name of the network driver plugin to use.  If not specified, uses the default configured on the daemon.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -EnableIPv6
Enables IPv6 support on the network.

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

### -IPAM
A Docker.DotNet.Models.IPAM object containing optional custom IP scheme settings for the network.

```yaml
Type: IPAM
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Internal
If specified, external access to the network will be restricted.

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

### -Labels
A dictionary containing labels to set on the network.

```yaml
Type: System.Collections.Generic.IDictionary`2[System.String,System.String]
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
The network name to use.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Options
A dictionary containing driver specific network options.

```yaml
Type: System.Collections.Generic.IDictionary`2[System.String,System.String]
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
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

[Online Version:](https://github.com/Microsoft/Docker-PowerShell/blob/master/src/Docker.PowerShell/Help/New-ContainerNet.md)
