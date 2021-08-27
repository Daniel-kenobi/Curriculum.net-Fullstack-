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
  lst_idiomas: Array<idiomas>;
  lst_qualidades: Array<qualidade>;
  template: e_template_curriculo;
}

export enum e_template_curriculo {
  classico,
  moderno
}

enum e_nivel_idioma {
  basico,
  intermediario,
  avancado,
  fluente
}

export class idiomas {
  Idioma: string;
  Nivel: e_nivel_idioma;
}

export class qualidade {
  Nome: string;
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
  atual: boolean = false;
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
