import { logging } from "protractor";

export default interface Machine{
    name?: string;
    ip?: string;
    antivirus?: string;
    windows_version?: string;
    is_firewall_active?: boolean;
    dotnet_version?: string;
    total_hd_size?: number;
    free_hd_size?: number;
} 