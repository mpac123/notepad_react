import React, { useState, useEffect } from "react";
import { apiService } from "./services";
import { DatePicker, Pagination, Select, Row, Col, message } from "antd";
import { Loader, Segment, Button } from "semantic-ui-react";
import moment from "moment";
import {
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableFooter,
  TableBody
} from "@material-ui/core";

const NotesList = ({
  page,
  setPage,
  title,
  setTitle,
  filters,
  setFilters,
  state,
  setState,
  setSelectedNote
}) => {
  const [notesList, setNotesList] = useState([]);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchNotes = async () => {
    setLoading(true);
    const notes = await apiService.getNotes(
      page,
      filters.dateFrom,
      filters.dateTo,
      filters.category
    );
    const category = await apiService.getCategories();
    console.log("notes", notes);
    console.log("categories", category);
    setNotesList(notes);
    setCategories(category);
    setLoading(false);
  };
  useEffect(() => {
    fetchNotes();
  }, [page, filters]);

  const onDateChange = (date, dateString) => {
    setPage(1);
    setFilters({ ...filters, dateFrom: dateString[0], dateTo: dateString[1] });
  };

  const onPageChange = (page, pageSize) => {
    setPage(page);
  };

  const onCategoryChange = value => {
    if (value === "0") {
      value = undefined;
    }
    setFilters({ ...filters, category: value });
  };

  const deleteNote = async id => {
    try {
      await apiService.deleteNote(id);
      message.success("Note has been deleted successfully");
    } catch {
      message.error("Note could not be deleted");
    }
    fetchNotes();
  };

  return (
    <>
      <Segment color="blue" padded secondary>
        <Row>
          <Col span={12}>Date range:</Col>
          <Col span={12}>Category:</Col>
        </Row>
        <Row>
          <Col span={12}>
            <DatePicker.RangePicker onChange={onDateChange} />
          </Col>
          <Col span={12}>
            <Select
              style={{ width: "120" }}
              placeholder="Select category"
              onChange={onCategoryChange}
              defaultValue="0"
            >
              <Select.Option value="0">All categories</Select.Option>
              {categories.map(c => (
                <Select.Option key={c.categoryID} value={c.categoryID}>
                  {c.title}
                </Select.Option>
              ))}
            </Select>
          </Col>
        </Row>
      </Segment>
      {loading ? (
        <Loader inverted>Loading</Loader>
      ) : (
        <>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Date</TableCell>
                <TableCell>Title</TableCell>
                <TableCell>Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {Object.values(notesList.notes).map(n => (
                <TableRow key={n.title}>
                  <TableCell>
                    {moment(n.dateTime).format("DD MMM YYYY")}
                  </TableCell>
                  <TableCell>{n.title}</TableCell>
                  <TableCell>
                    <Button
                      size="tiny"
                      onClick={() => {
                        setState("edit");
                        setSelectedNote(n);
                      }}
                      color="blue"
                    >
                      Edit
                    </Button>
                    |{" "}
                    <Button
                      size="tiny"
                      onClick={() => deleteNote(n.noteID)}
                      color="red"
                    >
                      Delete
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
          <Segment basic style={{ textAlign: "center" }}>
            <Pagination
              current={page}
              total={10 * notesList.allPages}
              onChange={onPageChange}
            />
          </Segment>
        </>
      )}
    </>
  );
};

export default NotesList;
