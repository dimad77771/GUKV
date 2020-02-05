using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace GUKV.DataMigration
{
    //public class DocumentFinder
    //{
    //    #region Member variables

    //    /// <summary>
    //    /// Logger, inherited from the Migrator object
    //    /// </summary>
    //    MigrationLog logger = null;

    //    /// <summary>
    //    /// Contains document IDs stored by a special key (formatted document number and date)
    //    /// </summary>
    //    private Dictionary<string, int> documentsByNumAndDate = new Dictionary<string, int>();

    //    /// <summary>
    //    /// Contains IDs of all existing documents
    //    /// </summary>
    //    private HashSet<int> existingDocuments = new HashSet<int>();

    //    /// <summary>
    //    /// Contains all links between documents (formatted into strings)
    //    /// </summary>
    //    private HashSet<string> docLinksToDocument = new HashSet<string>();

    //    /// <summary>
    //    /// Contains all links between documents and objects (formatted into strings)
    //    /// </summary>
    //    private HashSet<string> docLinksToObject = new HashSet<string>();

    //    /// <summary>
    //    /// Contains all links between documents and organizations (formatted into strings)
    //    /// </summary>
    //    private HashSet<string> docLinksToOrganization = new HashSet<string>();

    //    /// <summary>
    //    /// Contains IDs of all documents for which 'general kind' is defined
    //    /// </summary>
    //    private HashSet<int> documentsWithGeneralKind = new HashSet<int>();

    //    /// <summary>
    //    /// A cache of relations between documents and objects from the Privatization database
    //    /// </summary>
    //    private HashSet<string> privatObjDocLinks = new HashSet<string>();

    //    #endregion (Member variables)

    //    public DocumentFinder(MigrationLog log)
    //    {
    //        logger = log;
    //    }

    //    #region Interface

    //    public void RefreshDocumentCache(SqlConnection connection)
    //    {
    //        ///////////////////////////////////////////////////////////////////
    //        // 1) Get all documents from the 'documents' table
    //        ///////////////////////////////////////////////////////////////////

    //        string querySelect = "SELECT id, doc_num, doc_date, topic, kind_id, general_kind_id FROM documents";

    //        using (SqlCommand command = new SqlCommand(querySelect, connection))
    //        {
    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    int docId = (int)reader.GetValue(0);

    //                    string num = reader.IsDBNull(1) ? "" : ((string)reader.GetValue(1)).Trim();
    //                    DateTime dt = reader.IsDBNull(2) ? DateTime.MinValue : (DateTime)reader.GetValue(2);
    //                    string topic = reader.IsDBNull(3) ? "" : ((string)reader.GetValue(3)).Trim();
    //                    int kind = reader.IsDBNull(4) ? -1 : (int)reader.GetValue(4);

    //                    // This ID is already used
    //                    existingDocuments.Add(docId);

    //                    // Save the document ID by the special generated key
    //                    if (num.Length > 0 && dt != DateTime.MinValue && kind > 0)
    //                    {
    //                        string key = GetDocumentKey(dt, num, kind, topic);

    //                        if (!documentsByNumAndDate.ContainsKey(key))
    //                        {
    //                            documentsByNumAndDate.Add(key, docId);
    //                        }
    //                    }

    //                    // If general kind is defined, save this document
    //                    if (!reader.IsDBNull(5))
    //                    {
    //                        documentsWithGeneralKind.Add(docId);
    //                    }
    //                }

    //                reader.Close();
    //            }
    //        }

    //        ///////////////////////////////////////////////////////////////////
    //        // 2) Get all relations between documents and objects from the 'building_docs' table
    //        ///////////////////////////////////////////////////////////////////

    //        querySelect = "SELECT building_id, document_id FROM building_docs";

    //        using (SqlCommand command = new SqlCommand(querySelect, connection))
    //        {
    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    object dataObjId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                    object dataDocId = reader.IsDBNull(1) ? null : reader.GetValue(1);

    //                    if (dataObjId is int && dataDocId is int)
    //                    {
    //                        // Save the link by the special generated key
    //                        docLinksToObject.Add(GetDocLinkToObjectKey((int)dataObjId, (int)dataDocId));
    //                    }
    //                }

    //                reader.Close();
    //            }
    //        }

    //        ///////////////////////////////////////////////////////////////////
    //        // 3) Get all relations between documents 'doc_dependencies' table
    //        ///////////////////////////////////////////////////////////////////

    //        querySelect = "SELECT master_doc_id, slave_doc_id FROM doc_dependencies";

    //        using (SqlCommand command = new SqlCommand(querySelect, connection))
    //        {
    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    object dataParentId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                    object dataChildId = reader.IsDBNull(1) ? null : reader.GetValue(1);

    //                    if (dataParentId is int && dataChildId is int)
    //                    {
    //                        // Save the link by the special generated key
    //                        docLinksToDocument.Add(GetDocLinkToDocumentKey((int)dataParentId, (int)dataChildId));
    //                    }
    //                }

    //                reader.Close();
    //            }
    //        }
    //    }

    //    public int FindDocumentIn1NF(DateTime docDate, string docNum, int docKind, string docTopic)
    //    {
    //        int docId = -1;

    //        if (docNum.Trim().Length > 0 && docDate != DateTime.MinValue)
    //        {
    //            string key = GetDocumentKey(docDate, docNum, docKind, docTopic);

    //            if (documentsByNumAndDate.TryGetValue(key, out docId))
    //            {
    //                return docId;
    //            }
    //        }

    //        return -1;
    //    }

    //    public bool DocumentExistsInSQLServer(int documentId)
    //    {
    //        return existingDocuments.Contains(documentId);
    //    }

    //    public bool DocLinkToOrgExistsInSQLServer(int organizationId, int documentId)
    //    {
    //        return docLinksToOrganization.Contains(GetDocLinkToOrganizationKey(organizationId, documentId));
    //    }

    //    public bool DocLinkToObjExistsInSQLServer(int objectId, int documentId)
    //    {
    //        return docLinksToObject.Contains(GetDocLinkToObjectKey(objectId, documentId));
    //    }

    //    public bool DocLinkToDocExistsInSQLServer(int parentDocId, int childDocId)
    //    {
    //        return docLinksToDocument.Contains(GetDocLinkToDocumentKey(parentDocId, childDocId));
    //    }

    //    public void RegisterDocument(int docId, object docDate, object docNum, object docKind,
    //        object docTopic, bool generalKindDefined)
    //    {
    //        existingDocuments.Add(docId);

    //        if (docDate is DateTime && docNum is string && docKind is int)
    //        {
    //            DateTime dt = (DateTime)docDate;
    //            string num = ((string)docNum).Trim();
    //            int kind = (int)docKind;

    //            if (dt != DateTime.MinValue && num.Length > 0 && kind > 0)
    //            {
    //                string key = GetDocumentKey(dt, num, kind, docTopic);

    //                if (!documentsByNumAndDate.ContainsKey(key))
    //                {
    //                    documentsByNumAndDate.Add(key, docId);
    //                }
    //            }
    //        }

    //        if (generalKindDefined)
    //        {
    //            documentsWithGeneralKind.Add(docId);
    //        }
    //    }

    //    public void RegisterDocLinkToObject(int objectId, int documentId)
    //    {
    //        docLinksToObject.Add(GetDocLinkToObjectKey(objectId, documentId));
    //    }

    //    public void AddDocLinkToOrganization(SqlConnection connection,
    //        int organizationId, int documentId, int linkKind)
    //    {
    //        string query = "INSERT INTO org_docs (organization_id, document_id) VALUES (" +
    //                    organizationId.ToString() + ", " + documentId.ToString() + ")";

    //        try
    //        {
    //            using (SqlCommand command = new SqlCommand(query, connection))
    //            {
    //                command.ExecuteNonQuery();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            logger.WriteSQLError(query, ex.Message);

    //            // Abort migration correctly
    //            throw new MigrationAbortedException();
    //        }

    //        // Save this link for future lookup
    //        docLinksToOrganization.Add(GetDocLinkToOrganizationKey(organizationId, documentId));
    //    }

    //    public void AddDocLinkToDoc(SqlConnection connection,
    //        int parentDocId, int childDocId, object modifiedBy, object modifyDate, object dependKind)
    //    {
    //        if (!DocLinkToDocExistsInSQLServer(parentDocId, childDocId))
    //        {
    //            Dictionary<string, object> values = new Dictionary<string, object>();

    //            values["master_doc_id"] = parentDocId;
    //            values["slave_doc_id"] = childDocId;
    //            values["modified_by"] = modifiedBy;
    //            values["modify_date"] = modifyDate;
    //            values["depend_kind_id"] = dependKind;

    //            string queryInsert = Migrator.GetInsertStatement("doc_dependencies", values);

    //            try
    //            {
    //                using (SqlCommand commandInsert = new SqlCommand(queryInsert, connection))
    //                {
    //                    commandInsert.ExecuteNonQuery();
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                logger.WriteSQLError(queryInsert, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }

    //            // Save this link for future lookup
    //            docLinksToDocument.Add(GetDocLinkToDocumentKey(parentDocId, childDocId));
    //        }
    //    }

    //    public void AddPrivatizationDocLinkToObject(SqlConnection connection,
    //        int privatizationId, int documentId, object masterDocumentId, object modifiedBy, object modifyDate)
    //    {
    //        // Check if such link already exists
    //        string key = privatizationId.ToString() + "_@_" + documentId.ToString();

    //        if (!privatObjDocLinks.Contains(key))
    //        {
    //            privatObjDocLinks.Add(key);

    //            Dictionary<string, object> values = new Dictionary<string, object>();

    //            values["privatization_id"] = privatizationId;
    //            values["document_id"] = documentId;
    //            values["modified_by"] = modifiedBy;
    //            values["modify_date"] = modifyDate;

    //            if (masterDocumentId is int)
    //            {
    //                values["master_doc_id"] = (int)masterDocumentId;
    //            }

    //            string queryInsert = Migrator.GetInsertStatement("priv_object_docs", values);

    //            try
    //            {
    //                using (SqlCommand commandInsert = new SqlCommand(queryInsert, connection))
    //                {
    //                    commandInsert.ExecuteNonQuery();
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                logger.WriteSQLError(queryInsert, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }
    //    }

    //    public bool IsGeneralKindDefined(int documentId)
    //    {
    //        return documentsWithGeneralKind.Contains(documentId);
    //    }

    //    //public void ModifyDocGeneralKind(SqlConnection connection, int documentId, int generalKindId)
    //    //{
    //    //    string query = "UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase +
    //    //        ".dbo.documents SET general_kind_id = " + generalKindId.ToString() + " WHERE id = " + documentId.ToString();

    //    //    try
    //    //    {
    //    //        // Execute UPDATE
    //    //        using (SqlCommand commandUpdate = new SqlCommand(query, connection))
    //    //        {
    //    //            commandUpdate.ExecuteNonQuery();
    //    //        }
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        logger.WriteSQLError(query, ex.Message);

    //    //        // Abort migration correctly
    //    //        throw new MigrationAbortedException();
    //    //    }
    //    //}

    //    #endregion Interface

    //    #region Implementation

    //    private string GetDocumentKey(DateTime docDate, string docNum, int docKind, object docTopic)
    //    {
    //        return docDate.ToShortDateString() + "_@_" + docNum.Trim().ToUpper() + "_K_" + docKind.ToString();
    //    }

    //    private string GetDocLinkToDocumentKey(int parentDocId, int childDocId)
    //    {
    //        return parentDocId.ToString() + "_@_" + childDocId.ToString();
    //    }

    //    private string GetDocLinkToObjectKey(int objectId, int documentId)
    //    {
    //        return objectId.ToString() + "_@_" + documentId.ToString();
    //    }

    //    private string GetDocLinkToOrganizationKey(int organizationId, int documentId)
    //    {
    //        return organizationId.ToString() + "_@_" + documentId.ToString();
    //    }

    //    #endregion Implementation
    //}
}
