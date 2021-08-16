import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { CurriculumModel, dto_endereco, HistoricoProfissional, InfosAcademicas, SoftSkills } from '../../models/curriculo.model';
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
        ID: 0,
        Nome: [''],
        Email: [''],
        Telefone: [''],
        Endereco: this.frmBuilder.group({
          cep: [''],
          Logradouro: [''],
          Complemento: [''],
          Bairro: [''],
          Localidade: [''],
          UF: ['']
        }),
        FraseMotivacional: [''],
        Linkedin: [''],
        GitHub: [''],
        Instagram: [''],
        lst_infos_academicas: this.frmBuilder.array([]),
        lst_Historico_Profissional: this.frmBuilder.array([]),
        lst_soft_skills: this.frmBuilder.array([])
      })
  }

  fnc_adiciona_infos_academicas() {
    const infos = this.curriculoGroup.controls.lst_infos_academicas as FormArray;

    if (infos.length == 3)
      return;

    infos.push(this.frmBuilder.group(
      {
        Nome_instituicao: [''],
        TipoCurso: [''], 
        Curso: [''],
        Descricao_aprendizado: [''],
        DataInicio: [''],
        DataConclusao: ['']
      }))
  }

  fnc_adiciona_Historico_Profissional() {
    const infos = this.curriculoGroup.controls.lst_Historico_Profissional as FormArray;

    if (infos.length == 3)
      return;

    infos.push(this.frmBuilder.group(
      {
        Nome_instituicao: [''],
        Cargo: [''],
        Descricao_cargo: [''],
        DataInicio: [''],
        DataSaida: [''],
      }))
  }

  fnc_adiciona_lst_soft_skills() {
    const infos = this.curriculoGroup.controls.lst_soft_skills as FormArray;

    if (infos.length == 3)
      return;

    infos.push(this.frmBuilder.group(
      {
        Nome: [''],
        Descricao: ['']
      }))
  }

  ngOnInit(): void {

  }

  onSubmit() {
    const curriculo = new CurriculumModel();

    this.curriculo_service.fnc_cria_curriculo(curriculo).subscribe(x => { console.log(x) }, (err) => { console.log(err) });
  }
}
