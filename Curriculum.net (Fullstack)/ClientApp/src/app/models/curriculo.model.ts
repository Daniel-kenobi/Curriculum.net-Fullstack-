export class CurriculumModel {

  ID: number | string; // interno
  Nome: string;
  Email: string;
  Telefone: string;
  Endereco: dto_endereco;
  FraseMotivacional: string;
  Linkedin: string;
  GitHub: string;
  Instagram: string;
  lst_infos_academicas: Array<InfosAcademicas>;
  lst_Historico_Profissional: Array<HistoricoProfissional>;
  lst_soft_skills: Array<SoftSkills>;
}

export class InfosAcademicas {
  Nome_instituicao: string;
  TipoCurso: string;
  Curso: string;
  Descricao_aprendizado: string;
  DataInicio: Date;
  DataConclusao: Date;
}

export class HistoricoProfissional {
  Nome_instituicao: string;
  Cargo: string;
  Descricao_cargo: string;
  DataInicio: Date;
  DataSaida: Date;
}

export class dto_endereco {
  cep: string;
  Logradouro: string;
  Complemento: string;
  Bairro: string;
  Localidade: string;
  UF: string;
}

export class SoftSkills {
  Nome: string;
  Descricao: string;
}
