export default interface Command{
    text: string;
    output?: string;
    directory?: string;
    type: TerminalType
}

export enum TerminalType{
    Cmd,
    Powershell
}