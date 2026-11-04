@code {
    public string ModelName { get; set; } = "NeXhireDb";
}

<h2>@ModelName</h2>

<table>
    <tr><th>Table name</th><th>EF Core type (PostgresEnum)</th></tr>
    @foreach (var col in @Columns)
        <tr><td>@col.ColumnName</td><td>@IsEnum(col.Type) ? "PostgresEnum" : GetDbType(col.Type)</td></tr>
</table>

<details open="true">
  <summary>@ModelName – Full EF Core Configurations & Domain Classes (including Enums and Relations)</summary>

  <!-- ---------- EF Core Configuration Generated via Code ---------- -->
  @{
      string sql = "return new DbContextModelBuilder().Build();";
      System.IO.File.WriteAllText("~/Generated/Configurations.cs", sql);
      
      // In a real implementation, you would use code generation to build proper EF Core fluent API
      // Here we just simulate the logic: each table becomes an entity with key and properties
  }

  <!-- ---------- Domain Code Generated via Code ---------- -->
  @{
      string domainCode = "";
      
      // Empresa entity in domain
      var empresaDomain = new Company();
      foreach (var col in @ColumnsWhereTable == "empresa")
          empresaDomain.AddProperty(col.ColumnName, GetDbType(col.Type));
  
      if (@ContainsRelationColumns("setor_ocupacao"))
          empresaDomain.AddReference("setores", typeof(SetorOcupacao));
          
      // Usuario entity in domain
      var usuarioDomain = new User();
      foreach (var col in @ColumnsWhereTable == "usuario")
          usuarioDomain.AddProperty(col.ColumnName, GetDbType(col.Type));
      
      if (@ContainsRelationColumns("empresa_funcionario"))
          empresaDomain.AddReference("funcionarios", typeof(EmpresaFuncionario));
          
      // ExperienceProfessional entity in domain
      var expProfDom = new ExperienceProfessional();
      foreach (var col in @ColumnsWhereTable == "experiencia_profissional")
          expProfDom.AddProperty(col.ColumnName, GetDbType(col.Type));
      
      // Alternativa entity in domain (with enum handling)
      var alternativaDomain = new Alternative();
      foreach (var col in @ColumnsWhereTable == "alternativa")
          alternativaDomain.AddProperty(col.ColumnName, GetDbType(col.Type));
          
      if (@ContainsRelationColumns("alt_alternativa") && col.QuestionId != null)
          alternativaDomain.AddReference("questao", typeof(Questao));
      
      domainCode += empresaDomain.ToString();
      domainCode += usuarioDomain.ToString();
      domainCode += expProfDom.ToString();
      domainCode += alternativaDomain.ToString();

      System.IO.File.WriteAllText("~/Generated/DomainModels.cs", domainCode);
  }

  <function-call>
    // Helper methods (replace with real logic)
    function GetDbType(type) {
        if (type.Contains("_enum"))
            return "string"; // EF Core will map PostgreSQL ENUM to string
        else if (type.Contains("SERIAL") || type.Contains("TEXT") || type.Contains("VARCHAR"))
            return "string";
        else if (type.Contains("BOOLEAN")) 
            return "bool";
        else if (type.Contains("NUMERIC"))
            return "int";
        return "string";
    }

    function @IsEnum(type) {
        return type.StartsWith("CREATE TYPE ");
    }
    
    function ContainsRelationColumns(tableName)
    {
        // This is a placeholder - in real implementation you'd parse CREATE TABLE statements
        switch (tableName)
            case "alternativa":
                return true;
            default:
                return false;
        }
    }
  </function-call>

</details>
