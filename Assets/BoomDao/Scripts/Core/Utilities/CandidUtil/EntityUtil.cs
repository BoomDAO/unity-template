
using Boom.Utility;
using Boom.Values;
using Candid;
using Candid.World.Models;
using EdjCase.ICP.Candid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
// Ignore Spelling: Util

public static class EntityFilterPredicate
{
    public class Base { }

    public class Text : Base
    {
        public Predicate<string> Predicate { get; private set; }

        public Text(Predicate<string> predicate)
        {
            Predicate = predicate;
        }
    }
    public class Double : Base
    {
        public Predicate<double> Predicate { get; private set; }

        public Double(Predicate<double> predicate)
        {
            Predicate = predicate;
        }
    }
    public class ULong : Base
    {
        public Predicate<ulong> Predicate { get; private set; }

        public ULong(Predicate<ulong> predicate)
        {
            Predicate = predicate;
        }
    }
    public class Bool : Base
    {
        public Predicate<bool> Predicate { get; private set; }

        public Bool(Predicate<bool> predicate)
        {
            Predicate = predicate;
        }
    }
}
public static class EntityFilter
{
    public abstract class Base
    {

        public KeyValue<string, EntityFilterPredicate.Base>[] fieldsConditions;

        protected Base(KeyValue<string, EntityFilterPredicate.Base>[] fieldsConditions)
        {
            this.fieldsConditions = fieldsConditions;
        }
    }

    public class FromUser : Base
    {
        public string PrincipalId { get; private set; }

        public FromUser(string principal, params KeyValue<string, EntityFilterPredicate.Base>[] fieldCondition) : base(fieldCondition)
        {
            this.PrincipalId = principal;
        }
    }

    public class FromWorld : Base
    {
        public string PrincipalId { get { return CandidApiManager.Instance.WORLD_CANISTER_ID; } }

        public FromWorld(params KeyValue<string, EntityFilterPredicate.Base>[] fieldCondition) : base(fieldCondition)
        {
        }
    }

    public class FromSelf : Base
    {
        public FromSelf(params KeyValue<string, EntityFilterPredicate.Base>[] fieldCondition) : base(fieldCondition)
        {
        }
    }
}

internal static class EntityUtil
{

    public static class Queries
    {
        public static EntityFilter.FromWorld rooms = new EntityFilter.FromWorld(
                        new KeyValue<string, EntityFilterPredicate.Base>("tag", new EntityFilterPredicate.Text(e => e == "room")),
                        new KeyValue<string, EntityFilterPredicate.Base>("userCount", new EntityFilterPredicate.Double(e => e != 0.0)));

        public static EntityFilter.FromSelf ownItems = new EntityFilter.FromSelf(new KeyValue<string, EntityFilterPredicate.Base>("tag", new EntityFilterPredicate.Text(e => e == "item")));
    }


    public static bool TryGetAllEntitiesOf<T>(EntityFilter.Base entityFilter, out LinkedList<T> entities, Func<DataTypes.Entity, T> getter)
    {
        string principalId = "";
        KeyValue<string, EntityFilterPredicate.Base>[] conditions = null;

        switch (entityFilter)
        {
            case EntityFilter.FromUser e:
                principalId = e.PrincipalId;
                conditions = e.fieldsConditions;
                break;
            case EntityFilter.FromWorld e:
                principalId = e.PrincipalId;
                conditions = e.fieldsConditions;
                break;
            case EntityFilter.FromSelf e:
                principalId = "self";
                conditions = e.fieldsConditions;
                break;
        }

        entities = null;
        var elementsResult = UserUtil.GetElementsOfType<DataTypes.Entity>(principalId);

        if (elementsResult.IsErr) return false;

        if (conditions == null)
        {
            entities = new();
            var elements = elementsResult.AsOk();

            foreach (var e in elements)
            {
                entities.AddLast(getter.Invoke(e));
            }
        }
        else
        {
            entities = new();

            var entitiesToCheckOn = elementsResult.AsOk();

            foreach (var entity in entitiesToCheckOn)
            {
                foreach (var condition in conditions)
                {
                    if (entity.fields.TryGetValue(condition.key, out var value) == false)
                    {
                        goto entityLoopEnd;
                    }
                    else
                    {
                        if (condition.value != null)
                        {
                            switch (condition.value)
                            {
                                case EntityFilterPredicate.Text predicate:

                                    if (predicate.Predicate.Invoke(value) == false) goto entityLoopEnd;

                                    break;
                                case EntityFilterPredicate.Double predicate:

                                    if(value.TryParseValue<double>(out var decimalValue) == false)
                                    {
                                        Debug.LogError($"Issue parsing field of id: {condition.key}, from string to double");
                                        return false;
                                    } 

                                    if (predicate.Predicate.Invoke(decimalValue) == false) goto entityLoopEnd;

                                    break;
                                case EntityFilterPredicate.ULong predicate:

                                    if (value.TryParseValue<ulong>(out var ulongValue) == false)
                                    {
                                        Debug.LogError($"Issue parsing field of id: {condition.key}, from string to ulong");
                                        return false;
                                    }

                                    if (predicate.Predicate.Invoke(ulongValue) == false) goto entityLoopEnd;

                                    break;
                                case EntityFilterPredicate.Bool predicate:

                                    if (value.TryParseValue<bool>(out var boolValue) == false)
                                    {
                                        Debug.LogError($"Issue parsing field of id: {condition.key}, from string to bool");
                                        return false;
                                    }

                                    if (predicate.Predicate.Invoke(boolValue) == false) goto entityLoopEnd;

                                    break;
                            }
                        }
                    }
                }

                entities.AddLast(getter.Invoke(entity));

            entityLoopEnd: { }
            }
        }

        return true;
    }


    public static bool GetFieldAs<T>(string uid, string entityId, string fieldName, out T returnValue, T defaultValue = default)
    {
        returnValue = defaultValue;

        var result = UserUtil.GetElementOfType<DataTypes.Entity>(uid, entityId);
        if (result.IsErr)
        {
            return false;
        }

        if (!result.AsOk().fields.TryGetValue(fieldName, out var value)) return false;

        if (value.TryParseValue<T>(out returnValue))
        {
            return true;
        }
        Debug.LogError($"Error on \"value\" type, current type: {value.GetType()}, desired type is: {typeof(T)}");
        return false;
    }
    public static bool GetFieldAsString(string uid, string entityId, string fieldName, out string returnValue, string defaultValue = default)
    {
        return GetFieldAs<string>(uid, entityId, fieldName, out returnValue, defaultValue);
    }
    public static bool GetFieldAsDouble(string uid, string entityId, string fieldName, out double returnValue, double defaultValue = default)
    {
        return GetFieldAs<double>(uid, entityId, fieldName, out returnValue, defaultValue);
    }
    public static bool GetFieldAsULong(string uid, string entityId, string fieldName, out ulong returnValue, ulong defaultValue = default)
    {
        return GetFieldAs<ulong>(uid, entityId, fieldName, out returnValue, defaultValue);
    }
    private static bool GetFieldAs<T>(this DataTypes.Entity entity, string fieldName, out T returnValue, T defaultValue = default)
    {
        returnValue = defaultValue;

        if (!entity.fields.TryGetValue(fieldName, out var value)) return false;

        if (value.TryParseValue<T>(out returnValue))
        {
            return true;
        }
        Debug.LogError($"Error on \"value\" type, current type: {value.GetType()}, desired type is: {typeof(T)}");
        return false;
    }
    public static bool GetFieldAsString(this DataTypes.Entity entity, string fieldName, out string returnValue, string defaultValue = default)
    {
        return GetFieldAs<string>(entity, fieldName, out returnValue, defaultValue);
    }
    public static bool GetFieldAsDouble(this DataTypes.Entity entity, string fieldName, out double returnValue, double defaultValue = default)
    {
        return GetFieldAs<double>(entity, fieldName, out returnValue, defaultValue);
    }
    public static bool GetFieldAsULong(this DataTypes.Entity entity, string fieldName, out ulong returnValue, ulong defaultValue = default)
    {
        return GetFieldAs<ulong>(entity, fieldName, out returnValue, defaultValue);
    }
    //
    public static bool GetEditedFieldAsNumeber(this NewEntityEdits newEntityValues, string fieldName, out double returnValue, double defaultValue = default)
    {
        returnValue = defaultValue;

        if (!newEntityValues.fields.TryGetValue(fieldName, out var edit)) return false;

        switch (edit)
        {
            case EntityFieldEdit.Numeric e:

                switch (e.Value)
                {
                    case EntityFieldEdit.Numeric.ValueType.Number n:
                        returnValue = n.Value;
                        return true;
                    default:
                        Debug.LogError($"Error on \"value\" type, current type: numeric formula, desired type is: number for field: {fieldName}");
                        return false;
                }
            default:
                Debug.LogError($"Error on \"value\" type, current type: {edit.GetType()}, desired type is: {typeof(EntityFieldEdit.Numeric)} for field: {fieldName}");
                return false;
        }
    }
    public static bool GetEditedFieldAsText(this NewEntityEdits newEntityValues, string fieldName, out string returnValue, string defaultValue = default)
    {
        returnValue = defaultValue;

        if (!newEntityValues.fields.TryGetValue(fieldName, out var edit)) return false;

        switch (edit)
        {
            case EntityFieldEdit.SetText e:
                returnValue = e.Value;
                return true;
            default:
                Debug.LogError($"Error on \"value\" type, current type: {edit.GetType()}, desired type is: {typeof(EntityFieldEdit.SetText)} for field: {fieldName}");
                return false;
        }
    }
    public static bool GetEditedFieldAsOldText(this NewEntityEdits newEntityValues, string fieldName, out string returnValue, string defaultValue = default)
    {
        returnValue = defaultValue;

        if (!newEntityValues.fields.TryGetValue(fieldName, out var edit)) return false;

        switch (edit)
        {
            case EntityFieldEdit.ReplaceText e:
                returnValue = e.OldText;
                return true;
            default:
                Debug.LogError($"Error on \"value\" type, current type: {edit.GetType()}, desired type is: {typeof(EntityFieldEdit.ReplaceText)} for field: {fieldName}");
                return false;
        }
    }
    public static bool GetEditedFieldAsNewText(this NewEntityEdits newEntityValues, string fieldName, out string returnValue, string defaultValue = default)
    {
        returnValue = defaultValue;

        if (!newEntityValues.fields.TryGetValue(fieldName, out var edit)) return false;

        switch (edit)
        {
            case EntityFieldEdit.ReplaceText e:
                returnValue = e.NewText;
                return true;
            default:
                Debug.LogError($"Error on \"value\" type, current type: {edit.GetType()}, desired type is: {typeof(EntityFieldEdit.ReplaceText)} for field: {fieldName}");
                return false;
        }
    }

    //
    private static readonly FormulaEvaluation formulaEvaluation = new();

    public static void RefineActionOutcomes(this ProcessedActionResponse processedActionResponse, IEnumerable<Field> args)
    {
        IEnumerable<MainDataTypes.AllConfigs.Config> configs;

        IEnumerable<DataTypes.Entity> callerEntities;
        IEnumerable<DataTypes.Entity> targetEntities;
        IEnumerable<DataTypes.Entity> worldEntities;

        //CONFIG
        var configDataTypeResult = UserUtil.GetMainData<MainDataTypes.AllConfigs>();

        if (configDataTypeResult.IsErr)
        {
            Debug.LogError(configDataTypeResult.AsErr());
            return;
        }

        if (configDataTypeResult.AsOk().configs.TryGetValue(CandidApiManager.Instance.WORLD_CANISTER_ID, out var worldConfig) == false)
        {
            Debug.LogError("Could not find config for world of id: " + CandidApiManager.Instance.WORLD_CANISTER_ID);
            return;
        }

        configs = worldConfig.Map(e => e.Value);

        //CALLER
        if (processedActionResponse.callerOutcomes != null)
        {
            var entityDataTypeResult = UserUtil.GetDataSelf<DataTypes.Entity>();

            if (entityDataTypeResult.IsErr)
            {
                Debug.LogError(entityDataTypeResult.AsErr());
                return;
            }

            callerEntities = entityDataTypeResult.AsOk().elements.Map(e => e.Value);
        }
        else callerEntities = new DataTypes.Entity[0];


        //TARGET
        if (processedActionResponse.targetOutcomes != null)
        {
            var entityDataTypeResult = UserUtil.GetData<DataTypes.Entity>(processedActionResponse.targetOutcomes.uid);

            if (entityDataTypeResult.IsErr)
            {
                Debug.LogError(entityDataTypeResult.AsErr());
                return;
            }

            targetEntities = entityDataTypeResult.AsOk().elements.Map(e => e.Value);
        }
        else targetEntities = new DataTypes.Entity[0];


        //WORLD
        if (processedActionResponse.worldOutcomes != null)
        {
            var entityDataTypeResult = UserUtil.GetData<DataTypes.Entity>(processedActionResponse.worldOutcomes.uid);

            if (entityDataTypeResult.IsErr)
            {
                Debug.LogError(entityDataTypeResult.AsErr());
                return;
            }

            worldEntities = entityDataTypeResult.AsOk().elements.Map(e => e.Value);
        }
        else worldEntities = new DataTypes.Entity[0];


        if (processedActionResponse.callerOutcomes != null) {
            var entityEdits = processedActionResponse.callerOutcomes.entityEdits.Map(e => e.Value);
            foreach (var e in entityEdits)
                e.RefineEntityEdits(worldEntities, callerEntities, targetEntities, configs, args);
        }
        if (processedActionResponse.targetOutcomes != null)
        {
            var entityEdits = processedActionResponse.targetOutcomes.entityEdits.Map(e => e.Value);
            foreach (var e in entityEdits)
                e.RefineEntityEdits(worldEntities, callerEntities, targetEntities, configs, args);
        }
        if (processedActionResponse.worldOutcomes != null)
        {
            var entityEdits = processedActionResponse.worldOutcomes.entityEdits.Map(e => e.Value);
            foreach (var e in entityEdits)
                e.RefineEntityEdits(worldEntities, callerEntities, targetEntities, configs, args);
        }
    }
    public static void RefineEntityEdits(this NewEntityEdits newEntityValues, IEnumerable<DataTypes.Entity> worldEntities, IEnumerable<DataTypes.Entity> callerEntities, IEnumerable<DataTypes.Entity> targetEntities, IEnumerable<MainDataTypes.AllConfigs.Config> configs, IEnumerable<Field> args)
    {
        var fields = newEntityValues.fields;
        if (string.IsNullOrEmpty(newEntityValues.wid)) newEntityValues.wid = CandidApiManager.Instance.WORLD_CANISTER_ID;

        foreach (var pair in fields)
        {
            if(pair.Value is EntityFieldEdit.Numeric numeric)
            {
                if (numeric.Value is EntityFieldEdit.Numeric.ValueType.Formula formula)
                {
                    var formulaNonWhiteSpaces = formula.Value.Replace(" ", "");
                    
                    var formulaOutcome = EvaluateFormula(formulaNonWhiteSpaces, worldEntities, callerEntities, targetEntities, configs, args);
                    numeric.Value = new EntityFieldEdit.Numeric.ValueType.Number(formulaOutcome);

                    fields[pair.Key] = numeric;
                }
            }
        }
    }

    private static string ReplaceVariables(string formula, IEnumerable<DataTypes.Entity> worldEntities, IEnumerable<DataTypes.Entity> callerEntities, IEnumerable<DataTypes.Entity> targetEntities, IEnumerable<MainDataTypes.AllConfigs.Config> configs, IEnumerable<Field> args)
    {
        StringBuilder subExpr = new();
        bool isOpen = false;
        var index = 0;
        var returnValue = formula;
        while (index < formula.Length)
        {
            string token = $"{formula[index]}";


            if (isOpen)
            {
                subExpr.Append(token);
            }

            if (token == "{")
            {
                isOpen = true;
            }
            else if (index < formula.Length - 1)
            {
                string nextToken = $"{formula[index + 1]}";

                if (nextToken == "}")
                {
                    isOpen = false;
                }
            }

            if (!isOpen && subExpr.Length > 0)
            {
                string variable = subExpr.ToString();
                subExpr.Length = 0;

                var variableFieldNameElements = variable.Split('.');

                if(variableFieldNameElements.Length == 3)
                {
                    var source = variableFieldNameElements[0];
                    var key = variableFieldNameElements[1];
                    var fieldName = variableFieldNameElements[2];
                    var feildValue = "";
                    IEnumerable<DataTypes.Entity> entities = null;
                    if (source == "$caller") entities = callerEntities;
                    else if (source == "$target") entities = targetEntities;
                    else if (source == "$world") entities = worldEntities;
                    else if (source == "$config")
                    {
                        if (configs.TryLocate(e => e.cid == key, out var config))
                        {
                            if (!config.fields.TryGetValue(fieldName, out feildValue))
                            {
                                Debug.LogError($"Formula error, variable's value of id: {variable} could not be found");
                                feildValue = "Nan";
                            }
                            else
                            {
                                if (fieldName.Contains('@'))
                                {
                                    feildValue = EvaluateFormula(feildValue, worldEntities, callerEntities, targetEntities, configs, args).ToString();
                                }
                            }
                        }
                        returnValue = returnValue.Replace("{" + $"{variable}" + "}", $"{feildValue}");
                    }

                    if (entities.TryLocate(e => e.eid == key, out var entity))
                    {
                        if (! entity.fields.TryGetValue(fieldName, out feildValue))
                        {
                            Debug.LogError($"Formula error, variable's value of id: {variable} could not be found");
                            feildValue = "Nan";
                        }
                    }
                    returnValue = returnValue.Replace("{" + $"{variable}" + "}", $"{feildValue}");
                }
                else if (variableFieldNameElements.Length == 2)
                {
                    if(variableFieldNameElements[0] == "$args")
                    {
                        var actionArgFieldName = variableFieldNameElements[1];

                        if(args.TryLocate(e => e.FieldName == actionArgFieldName, out var argValue) == false)
                        {
                            return "Could not find arg value of field name: "+ actionArgFieldName;
                        }

                        returnValue = returnValue.Replace("{" + $"{variable}" + "}", $"{argValue}");
                    }
                }
            }


            index += 1;
        }

        return returnValue;
    }
    private static double EvaluateFormula(string formula, IEnumerable<DataTypes.Entity> worldEntities, IEnumerable<DataTypes.Entity> callerEntities, IEnumerable<DataTypes.Entity> targetEntities, IEnumerable<MainDataTypes.AllConfigs.Config> configs, IEnumerable<Field> args)
    {
        var formulaWithVariableReplaced = ReplaceVariables(formula, worldEntities, callerEntities, targetEntities, configs, args);
        return formulaEvaluation.Evaluate(formulaWithVariableReplaced);
    }

    internal static void ApplyEntityEdits(ProcessedActionResponse.Outcomes callerOutcomes)
    {
        var uid = callerOutcomes.uid;
        var entityEdits = callerOutcomes.entityEdits;

        Dictionary<string, DataTypes.Entity> editedEntities = new();


        foreach(var entity in entityEdits)
        {
            var eid = entity.Value.eid;
            var wid = entity.Value.wid;
            var fieldsEdits = entity.Value.fields;

            Dictionary<string, string> currentEntityFields = null;

            var currentEntityDataTypeResult = UserUtil.GetElementOfType<DataTypes.Entity>(uid, eid);

            if (currentEntityDataTypeResult.IsOk) currentEntityFields = currentEntityDataTypeResult.AsOk().fields;
            else currentEntityFields = new();



            foreach ( var edit in fieldsEdits) 
            {
                var fieldId = edit.Key;

                switch (edit.Value)
                {
                    case EntityFieldEdit.SetText e:
                        currentEntityFields[fieldId] = e.Value;
                        break;

                    case EntityFieldEdit.ReplaceText e:
                        currentEntityFields[fieldId] = currentEntityFields[fieldId].Replace(e.OldText, e.NewText);
                        break;

                    case EntityFieldEdit.SetNumber e:
                        currentEntityFields[fieldId] = e.Value.ToString();
                        break;
                    case EntityFieldEdit.IncrementNumber e:

                        switch (e.Value)
                        {
                            case EntityFieldEdit.Numeric.ValueType.Number ne:
                                if (currentEntityFields.TryGetValue(fieldId, out var numberAsText) == false) numberAsText = "0";
                                numberAsText.TryParseValue(out double currentNumericValue);

                                currentEntityFields[fieldId] = (currentNumericValue + ne.Value).ToString();

                                break;
                            default:
                                Debug.LogError($"Cannot apply formula to entity of id: {eid} and field of id: {fieldId}");
                                break;
                        }

                        break;

                    case EntityFieldEdit.DecrementNumber e:

                        switch (e.Value)
                        {
                            case EntityFieldEdit.Numeric.ValueType.Number ne:
                                if (currentEntityFields.TryGetValue(fieldId, out var numberAsText) == false) numberAsText = "0";
                                numberAsText.TryParseValue(out double currentNumericValue);

                                currentEntityFields[fieldId] = (currentNumericValue - ne.Value).ToString();

                                break;
                            default:
                                Debug.LogError($"Cannot apply formula to entity of id: {eid} and field of id: {fieldId}");
                                break;
                        }

                        break;

                    case EntityFieldEdit.RenewTimestamp e:

                        switch (e.Value)
                        {
                            case EntityFieldEdit.Numeric.ValueType.Number ne:

                                currentEntityFields[fieldId] = ne.Value.ToString();

                                break;
                            default:
                                Debug.LogError($"Cannot apply formula to entity of id: {eid} and field of id: {fieldId}");
                                break;
                        }

                        break;
                }
            }

            var newEditedEntity = new DataTypes.Entity(wid, eid, currentEntityFields);

            editedEntities[eid] = newEditedEntity;
        }

        if(editedEntities.Count > 0) UserUtil.UpdateData<DataTypes.Entity>(uid, editedEntities.ToArray().Map(e => e.Value).ToArray());
    }
}

public class FormulaEvaluation
{
    string specialTokens = "()^*/%+-<>";
    private string[] operators = { "-", "+", "%", "/", "*", "^", "<", ">" };
    private Func<double, double, double>[] _operations = {
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 % a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2),
        (a1, a2) => Math.Min(a1, a2),
        (a1, a2) => Math.Max(a1, a2)
    };

    public double Evaluate(string expression)
    {
        List<string> tokens = getTokens(expression);
        Stack<double> operandStack = new Stack<double>();
        Stack<string> operatorStack = new Stack<string>();
        int tokenIndex = 0;

        while (tokenIndex < tokens.Count)
        {
            //
            string token = tokens[tokenIndex];
            if (token == "(")
            {
                string subExpr = GetSubExpression(tokens, ref tokenIndex);
                operandStack.Push(Evaluate(subExpr));
                continue;
            }
            if (token == ")")
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }
            //
            if (Array.IndexOf(operators, token) >= 0)
            {
                if (operatorStack.Count > 0 && Array.IndexOf(operators, token) < Array.IndexOf(operators, operatorStack.Peek()))
                {
                    while (operatorStack.Count > 0)
                    {
                        string op = operatorStack.Pop();
                        double arg2 = operandStack.Pop();
                        double arg1 = operandStack.Pop();
                        operandStack.Push(_operations[Array.IndexOf(operators, op)](arg1, arg2));
                    }
                }

                operatorStack.Push(token);
            }
            else
            {
                operandStack.Push(double.Parse(token));
            }
            //
            tokenIndex += 1;
        }
        //
        while (operatorStack.Count > 0)
        {
            string op = operatorStack.Pop();
            double arg2 = operandStack.Pop();
            double arg1 = operandStack.Pop();
            operandStack.Push(_operations[Array.IndexOf(operators, op)](arg1, arg2));
        }
        //
        return operandStack.Pop();
    }

    private string GetSubExpression(List<string> tokens, ref int index)
    {
        StringBuilder subExpr = new StringBuilder();
        int parenLevels = 1;
        index += 1;
        while (index < tokens.Count && parenLevels > 0)
        {
            string token = tokens[index];
            if (token == "(")
            {
                parenLevels += 1;
            }

            if (token == ")")
            {
                parenLevels -= 1;
            }

            if (parenLevels > 0)
            {
                subExpr.Append(token);
            }

            index += 1;
        }

        if (parenLevels > 0)
        {
            throw new ArgumentException("Mis-matched parentheses in expression");
        }
        return subExpr.ToString();
    }

    private List<string> getTokens(string expression)
    {

        List<string> tokens = new List<string>();
        StringBuilder sb = new StringBuilder();

        foreach (char c in expression.Replace(" ", string.Empty))
        {
            if (specialTokens.IndexOf(c) >= 0)
            {
                if ((sb.Length > 0))
                {
                    tokens.Add(sb.ToString());
                    sb.Length = 0;
                }
                tokens.Add($"{c}");
            }
            else
            {
                sb.Append(c);
            }
        }

        if ((sb.Length > 0))
        {
            tokens.Add(sb.ToString());
        }
        return tokens;
    }
}