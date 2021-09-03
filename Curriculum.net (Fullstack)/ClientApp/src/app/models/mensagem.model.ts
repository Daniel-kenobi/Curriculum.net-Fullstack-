import { usuarioModel } from "./usuario.model";

export class mensagemModel {
  codigo: number | string;
  usr: usuarioModel;
  Mensagem: string;
  respondida: boolean;
}
