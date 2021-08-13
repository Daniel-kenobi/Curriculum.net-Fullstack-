using  lib.dto;

namespace lib.Interfaces
{
    /// <summary>Interface modelo da classe de lógica de negócios (BLL) </summary>
    public interface IBllInterface<T> where T : class
    {
        void bll_vld(dto_curriculo adt);
        bool bll_criaCurriculum(dto_curriculo adt);
        string bll_retornaHTMLCurricullum(dto_curriculo adt);
    }
}
