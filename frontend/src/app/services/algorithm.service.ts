import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AlgorithmSummaryDto } from '../models/algorithm-summary.dto';
import { AlgorithmDetailDto } from '../models/algorithm-detail.dto';
import { CommentAlgorithmDto } from '../models/comment-algorithm.dto';

@Injectable({ providedIn: 'root' })
export class AlgorithmService {
  private api = 'http://localhost:5131/api/algorithms';

  constructor(private http: HttpClient) { }

  getAlgorithms(
    filter: string,
    page: number = 1,
    pageSize: number = 5,
    showPrivates: boolean = false,
    userId?: number
  ): Observable<AlgorithmSummaryDto[]> {
    let url = `${this.api}/summary?filter=${filter}&page=${page}&pageSize=${pageSize}&showPrivates=${showPrivates}`;

    if (userId !== undefined) {
      url += `&userId=${userId}`;
    }

    return this.http.get<AlgorithmSummaryDto[]>(url);
  }
  searchAlgorithms(query: string, page: number, pageSize: number): Observable<AlgorithmSummaryDto[]> {
    return this.http.get<AlgorithmSummaryDto[]>(
      `${this.api}/algorithms/search?query=${encodeURIComponent(query)}&page=${page}&pageSize=${pageSize}`
    );
  }
  getAlgorithmById(id: number): Observable<AlgorithmDetailDto> {
    return this.http.get<AlgorithmDetailDto>(`${this.api}/algorithms/${id}`);
  }
  getLatestCodeFilePath(algorithmId: number, language: string): Observable<{ path: string }> {
    return this.http.get<{ path: string }>(
      `${this.api}/latest-static-file?algorithmId=${algorithmId}&lang=${encodeURIComponent(language)}`
    );
  }
  commentAlgorithm(dto: CommentAlgorithmDto): Observable<any> {
  return this.http.post(`${this.api}/comment`, dto);
}
}