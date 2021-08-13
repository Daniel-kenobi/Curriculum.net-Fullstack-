import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CurriculumModel, HistoricoProfissional, InfosAcademicas, SoftSkills } from '../../models/curriculo.model';
import { cepService } from '../../services/cep.service';
import { curriculoService } from '../../services/curriculo.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
  curriculoGroup: FormGroup;
  curriculo: CurriculumModel = new CurriculumModel();

  constructor(private frmBuilder: FormBuilder, private cep_service: cepService, private curriculo_service: curriculoService) {
    this.curriculoGroup = this.frmBuilder.group(
      {
        Nome: [''],
        Email: [''],
        Telefone: [''],
        CEP: [''],
        FraseMotivacional: [''],
        Linkedin: [''],
        GitHub: [''],
        Instagram: [''],
        lst_infos_academicas: new FormBuilder().array([
          {
            Nome_instituicao: [''],
            TipoCurso: [''],
            Curso: [''],
            Descricao_aprendizado: [''],
            DataInicio: [''],
            DataConclusao: [''],
          }
        ]),
        lst_Historico_Profissional: new FormBuilder().array([
          {
            Nome_empresa: [''],
            Cargo: [''],
            Descricao_cargo: [''],
            emp_DataInicio: [''],
            emp_DataSaida: [''],
          }
        ]),
        lst_soft_skills: new FormBuilder().array([
          {
            Nome: [''],
            Descricao: ['']
          }
        ])
      })
  }

  ngOnInit(): void {

  }

  onSubmit() {

    var cep = this.curriculoGroup.get('CEP').value;
    this.cep_service.fnc_busca_cep(cep).subscribe(x => { this.curriculo.Endereco = x})

    this.curriculo.Nome = this.curriculoGroup.get('Nome').value;
    this.curriculo.Email = this.curriculoGroup.get('Email').value;
    this.curriculo.Telefone = this.curriculoGroup.get('Telefone').value;
    this.curriculo.Endereco = this.curriculoGroup.get('Endereco').value;
    this.curriculo.FraseMotivacional = this.curriculoGroup.get('FraseMotivacional').value;
    this.curriculo.Linkedin = this.curriculoGroup.get('Linkedin').value;
    this.curriculo.GitHub = this.curriculoGroup.get('GitHub').value;
    this.curriculo.Instagram = this.curriculoGroup.get('Instagram').value;
    this.curriculo.lst_infos_academicas = new Array<InfosAcademicas>();
    this.curriculo.lst_Historico_Profissional = new Array<HistoricoProfissional>();
    this.curriculo.lst_soft_skills = new Array<SoftSkills>();

    console.log(this.curriculo);

    // busca cep

    // envia curriculo
    this.curriculo_service.fnc_cria_curriculo(this.curriculo).subscribe(x => { console.log(x) }, (err) => { console.log(err) });
  }
}
