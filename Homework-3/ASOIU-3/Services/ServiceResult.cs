namespace ASOIU_3.Services;

// ServiceResult — вспомогательный тип результата бизнес-операции, а не Entity.
// Он позволяет одинаково обработать успех или ошибку в Console и WinForms.
internal readonly record struct ServiceResult(bool IsSuccess, string Message)
{
    // Статические фабричные методы скрывают детали создания результата
    // и делают вызывающий CRUD-код выразительнее.
    public static ServiceResult Ok(string message) => new(true, message);

    public static ServiceResult Fail(string message) => new(false, message);
}
