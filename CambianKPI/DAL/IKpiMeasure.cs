using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CambianKPI.DAL.ViewModels;
using CambianKPI.DAL.Models;


namespace CambianKPI.DAL
{
    public interface IKpiMeasure
    {
        #region CRUD
        KpiMeasure Create();
        KpiMeasure Read(int KpiMeasureID);
        void Update(KpiMeasure k);
        void Delete(KpiMeasure k);
        #endregion

        #region Methods
        List<AffectedMeasure> GetAffectedMeasures(int KpiMeasuredID);
        KpiMeasure GetExtendedMeasureWithFunction(int MeasureID);
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class AffectedMeasure
    {
        public int KpiMeasureID;
        public KpiFunction KpiFunction;
        public int KpiFunctionID;
        public int InputNumber;
        public int InputKpiMeasureID;
    }

}
